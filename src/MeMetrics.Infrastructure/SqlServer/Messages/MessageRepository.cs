using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using MeMetrics.Application.Interfaces;
using MeMetrics.Domain.Models;
using MeMetrics.Domain.Models.Messages;
using MeMetrics.Infrastructure.Helpers;
using MeMetrics.Infrastructure.SqlServer.Entities;
using MeMetrics.Infrastructure.SqlServer.Messages.Sql;
using MeMetrics.Infrastructure.SqlServer.Sql;
using Microsoft.Extensions.Options;

namespace MeMetrics.Infrastructure.SqlServer.Messages
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IOptions<EnvironmentConfiguration> _configuration;
        private readonly IEntityMapper _entityMapper;
        private readonly IMapper _mapper;

        public MessageRepository(
            IEntityMapper entityMapper,
            IOptions<EnvironmentConfiguration> configuration,
            IMapper mapper)
        {
            _entityMapper = entityMapper;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<List<Message>> GetMessages()
        {
            using (var connection = SqlConnectionBuilder.Build(_configuration.Value.DB_CONNECTION_STRING))
            {
                var messages = await connection.QueryAsync<Message>(Sql.GetMessages.Value);
                return messages.ToList();
            }
        }

        public async Task<MessageMetrics> GetOverviewMessageMetrics(
            DateTime? startDate, 
            DateTime endDate,
            DateTime? previousPeriodStartDate, 
            DateTime? previousPeriodEndDate)
        {
            using (var connection = SqlConnectionBuilder.Build(_configuration.Value.DB_CONNECTION_STRING))
            {
                if (!startDate.HasValue)
                {
                    // The only case for this is the All Time selection
                    // Dapper seems to have some optimizations that break and can potentially time out if you pass in parameters
                    // and then change those parameters in the sql that is run, so this is being run separately
                    using (var firstDatePoint = connection.QuerySingleAsync<DateTime?>(GetFirstDataPoint.Value))
                    {
                       startDate = previousPeriodEndDate = previousPeriodStartDate = await firstDatePoint;
                    }
                }

                var sql =
                    $@"{GetCalendarDatesInRange.Value(previousPeriodStartDate)}
                    {GetTotalsByGender.Value}
                    {GetTotal.Value}
                    {GetTotalsByDayOfWeek.Value}
                    {GetTotalsByHour.Value}
                    {GetTotalIncomingOutgoing.Value}
                    {GetTotalsByPeriod.Value}
                    {GetTotalsByWeek.Value}";

                using (var multi = connection.QueryMultiple(sql,
                    new
                    {
                        startDate,
                        endDate,
                        previousPeriodStartDate,
                        previousPeriodEndDate,
                    }))
                {
                    var days = await multi.ReadAsync<DateTime>();
                    var messageByGenderDtos = await multi.ReadAsync<MessageByGenderEntity>();
                    var totalDto = await multi.ReadSingleAsync<TotalEntity>();
                    var messagesByDayOfWeekDtos = await multi.ReadAsync<PerDayOfWeekEntity>();
                    var messagesByHourDtos = await multi.ReadAsync<PerHourEntity>();
                    var messagesInVsOutDtos = await multi.ReadAsync<InVsOutDto>();
                    var messagePerformanceDtos = await multi.ReadAsync<PerformanceEntity>();
                    var messageWeekOverWeek = await multi.ReadAsync<WeekOverWeekEntity>();
                    var metrics = new MessageMetrics
                    {
                        AverageOutgoingTextLengthFemale = _entityMapper.GetAverageTextLengthByGender(messageByGenderDtos.ToList(), "F"),
                        AverageOutgoingTextLengthMale = _entityMapper.GetAverageTextLengthByGender(messageByGenderDtos.ToList(), "M"),
                        TotalMessagesFemale = _entityMapper.GetTotalMessagesByGender(messageByGenderDtos.ToList(), "F"),
                        TotalMessagesMale = _entityMapper.GetTotalMessagesByGender(messageByGenderDtos.ToList(), "M"),
                        MessagesByDayOfWeek = _entityMapper.BuildByDayOfWeek<MessageByDayOfWeek>(messagesByDayOfWeekDtos.ToList()),
                        MessagesByHour = _entityMapper.BuildByHour<MessageByHour>(messagesByHourDtos.ToList()),
                        MessagePerformance = _entityMapper.BuildByPeriod(days.ToList(), messagePerformanceDtos.ToList(), previousPeriodEndDate),
                        WeekOverWeek = _mapper.Map<List<ByWeek>>(messageWeekOverWeek),
                        TotalMessages = totalDto.Total,
                        UniqueContacts = totalDto.UniqueContacts,
                        TotalMessagesIncoming = messagesInVsOutDtos.FirstOrDefault(d => d.IsIncoming)?.Total,
                        TotalMessagesOutgoing = messagesInVsOutDtos.FirstOrDefault(d => !d.IsIncoming)?.Total
                    };
                    return metrics;
                }
            }

        }

        public async Task<int> InsertMessages(IList<Message> messages)
        {
            var tvp = new DataTable();

            tvp.Columns.Add(nameof(Message.MessageId), typeof(string));
            tvp.Columns.Add(nameof(Message.PhoneNumber), typeof(string));
            tvp.Columns.Add(nameof(Message.Name), typeof(string));
            tvp.Columns.Add(nameof(Message.OccurredDate), typeof(DateTimeOffset));
            tvp.Columns.Add(nameof(Message.IsIncoming), typeof(bool));
            tvp.Columns.Add(nameof(Message.Text), typeof(string));
            tvp.Columns.Add(nameof(Message.IsMedia), typeof(bool));
            tvp.Columns.Add(nameof(Message.TextLength), typeof(int));
            tvp.Columns.Add(nameof(Message.ThreadId), typeof(string));

            foreach (var message in messages)
            {
                var row = tvp.NewRow();
                row[nameof(message.MessageId)] = message.MessageId;
                row[nameof(message.PhoneNumber)] = message.PhoneNumber;
                row[nameof(message.Name)] = message.Name;
                row[nameof(message.OccurredDate)] = message.OccurredDate;
                row[nameof(message.IsIncoming)] = message.IsIncoming;
                row[nameof(message.Text)] = message.Text;
                row[nameof(message.IsMedia)] = message.IsMedia;
                row[nameof(message.TextLength)] = message.TextLength;
                row[nameof(message.ThreadId)] = message.ThreadId;

                tvp.Rows.Add(row);
            }
            using (var connection = SqlConnectionBuilder.Build(_configuration.Value.DB_CONNECTION_STRING))
            {
                await connection.ExecuteAsync(MergeMessage.Value, new {tvp = tvp.AsTableValuedParameter("dbo.MessageType") });
            }

            return 1;
        }
    }
}
