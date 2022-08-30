using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using MeMetrics.Application.Interfaces;
using MeMetrics.Application.Models;
using MeMetrics.Domain.Models;
using MeMetrics.Domain.Models.RecruitmentMessage;
using MeMetrics.Infrastructure.Helpers;
using MeMetrics.Infrastructure.SqlServer.Entities;
using MeMetrics.Infrastructure.SqlServer.RecruitmentMessages.Sql;
using MeMetrics.Infrastructure.SqlServer.Sql;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;

namespace MeMetrics.Infrastructure.SqlServer.RecruitmentMessages
{
    public class RecruitmentMessageRepository : IRecruitmentMessageRepository
    {
        private readonly IEntityMapper _entityMapper;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IOptions<EnvironmentConfiguration> _configuration;


        public RecruitmentMessageRepository(
            IOptions<EnvironmentConfiguration> configuration,
            IEntityMapper entityMapper,
            ILogger logger,
            IMapper mapper)
        {
            _configuration = configuration;
            _entityMapper = entityMapper;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<RecruitmentMessage>> GetRecruitmentMessages()
        {
            using (var connection = SqlConnectionBuilder.Build(_configuration.Value.DB_CONNECTION_STRING))
            {
                var calls = await connection.QueryAsync<RecruitmentMessage>(Sql.GetRecruitmentMessages.Value);
                return calls.ToList();
            }
        }

        public async Task<int> InsertRecruitmentMessages(IList<RecruitmentMessage> messages)
        {
            try {

            var tvp = new DataTable();

            tvp.Columns.Add(nameof(RecruitmentMessage.RecruitmentMessageId), typeof(string));
            tvp.Columns.Add(nameof(RecruitmentMessage.RecruiterId), typeof(string));
            tvp.Columns.Add(nameof(RecruitmentMessage.MessageSource), typeof(int));
            tvp.Columns.Add(nameof(RecruitmentMessage.RecruiterName), typeof(string));
            tvp.Columns.Add(nameof(RecruitmentMessage.RecruiterCompany), typeof(string));
            tvp.Columns.Add(nameof(RecruitmentMessage.Subject), typeof(string));
            tvp.Columns.Add(nameof(RecruitmentMessage.Body), typeof(string));
            tvp.Columns.Add(nameof(RecruitmentMessage.IsIncoming), typeof(bool));
            tvp.Columns.Add(nameof(RecruitmentMessage.OccurredDate), typeof(DateTimeOffset));

            foreach (var message in messages)
            {
                var row = tvp.NewRow();
                row[nameof(message.RecruitmentMessageId)] = message.RecruitmentMessageId;
                row[nameof(message.RecruiterId)] = message.RecruiterId;
                row[nameof(message.MessageSource)] = message.MessageSource;
                row[nameof(message.RecruiterName)] = message.RecruiterName;
                row[nameof(message.RecruiterCompany)] = message.RecruiterCompany;
                row[nameof(message.Subject)] = message.Subject;
                row[nameof(message.Body)] = message.Body;
                row[nameof(message.IsIncoming)] = message.IsIncoming;
                row[nameof(message.OccurredDate)] = message.OccurredDate;

                tvp.Rows.Add(row);
            }
               using (var connection = SqlConnectionBuilder.Build(_configuration.Value.DB_CONNECTION_STRING))
                {
                    await connection.ExecuteAsync(MergeRecruitmentMessage.Value, new {tvp = tvp.AsTableValuedParameter("dbo.RecruitmentMessageType") });
                } 
            }
            catch(Exception e) {
                foreach (var message in messages)
                {
                    _logger.Information(JsonConvert.SerializeObject(message));
                }
                throw e;
            }

            return 1;
        }

        public async Task<RecruitmentMessageMetrics> GetOverviewRecruitmentMessageMetrics(
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
                    using (var firstDataPoint = connection.QueryFirstAsync<DateTime?>(GetFirstDataPoint.Value))
                    {
                        startDate = previousPeriodEndDate = previousPeriodStartDate = await firstDataPoint;
                    }
                }

                var sql = $@"{GetCalendarDatesInRange.Value(previousPeriodStartDate)}
                        {GetTotal.Value}
                        {GetTotalsByDayOfWeek.Value}
                        {GetTotalsByPeriod.Value}
                        {GetTotalsByWeek.Value}";

                using (var multi = await connection.QueryMultipleAsync(sql, new
                {
                    startDate,
                    endDate,
                    previousPeriodStartDate,
                    previousPeriodEndDate,
                }))
                {

                    var days = await multi.ReadAsync<DateTime>();
                    var totalDto = await multi.ReadSingleAsync<TotalEntity>();
                    var messagesByDayOfWeekDtos = await multi.ReadAsync<PerDayOfWeekEntity>();
                    var messagePerformanceDtos = await multi.ReadAsync<PerformanceEntity>();
                    var messageWeekOverWeek = await multi.ReadAsync<WeekOverWeekEntity>();

                    return new RecruitmentMessageMetrics()
                    {
                        MessagesByDayOfWeek =_entityMapper.BuildByDayOfWeek<RecruitmentMessageByDayOfWeek>(messagesByDayOfWeekDtos.ToList()),
                        MessagePerformance =_entityMapper.BuildByPeriod(days.ToList(), messagePerformanceDtos.ToList(), previousPeriodEndDate),
                        WeekOverWeek = _mapper.Map<List<ByWeek>>(messageWeekOverWeek),
                        TotalMessages = totalDto.Total,
                        UniqueContacts = totalDto.UniqueContacts,
                    };
                }
            }
        }
    }
}
