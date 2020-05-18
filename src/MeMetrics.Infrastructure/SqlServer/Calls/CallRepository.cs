using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using MeMetrics.Application.Interfaces;
using MeMetrics.Domain.Models;
using MeMetrics.Domain.Models.Calls;
using MeMetrics.Infrastructure.Helpers;
using MeMetrics.Infrastructure.SqlServer.Calls.Sql;
using MeMetrics.Infrastructure.SqlServer.Entities;
using MeMetrics.Infrastructure.SqlServer.Sql;
using Microsoft.Extensions.Options;

namespace MeMetrics.Infrastructure.SqlServer.Calls
{
    public class CallRepository : ICallRepository
    {
        private readonly IOptions<EnvironmentConfiguration> _configuration;
        private readonly IEntityMapper _entityMapper;
        private readonly IMapper _mapper;

        public CallRepository(
            IOptions<EnvironmentConfiguration> configuration,
            IEntityMapper entityMapper,
            IMapper mapper)
        {
            _configuration = configuration;
            _entityMapper = entityMapper;
            _mapper = mapper;
        }

        public async Task<List<Call>> GetCalls()
        {
            using (var connection = SqlConnectionBuilder.Build(_configuration.Value.DB_CONNECTION_STRING))
            {
                var sql = $@"{Sql.GetCalls.Value}";
                var calls = await connection.QueryAsync<Call>(sql);
                return calls.ToList();
            }
        }

        public async Task<int> InsertCall(Call call)
        {
            using (var connection = SqlConnectionBuilder.Build(_configuration.Value.DB_CONNECTION_STRING))
            {
                var sql = $@"{MergeCall.Value}";
                await connection.ExecuteAsync(sql, new
                {
                    call.CallId,
                    call.PhoneNumber,
                    call.Name,
                    call.OccurredDate,
                    call.IsIncoming,
                    call.Duration
                });
            }

            return 1;
        }

        public async Task<CallMetrics> GetOverviewCallMetrics(
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
                    {GetTotal.Value}
                    {GetTotalDuration.Value}
                    {GetTotalKnownFemaleDuration.Value}
                    {GetTotalKnownMaleDuration.Value}
                    {GetTotalsByDayOfWeek.Value}
                    {GetTotalsByHour.Value}
                    {GetTotalIncomingOutgoing.Value}
                    {GetTotalsByPeriod.Value}
                    {GetTotalsByWeek.Value}";

                using (var multi = connection.QueryMultiple(sql, new
                {
                    startDate,
                    endDate,
                    previousPeriodStartDate,
                    previousPeriodEndDate,
                }))
                {
                    var days = await multi.ReadAsync<DateTime>();
                    var totalCalls = await multi.ReadSingleAsync<int>();
                    var totalDuration = await multi.ReadSingleAsync<int>();
                    var totalKnownFemaleDuration = await multi.ReadSingleAsync<int>();
                    var totalKnownMaleDuration = await multi.ReadSingleAsync<int>();
                    var callsByDayOfWeekDto = await multi.ReadAsync<PerDayOfWeekEntity>();
                    var callsByHourDto = await multi.ReadAsync<PerHourEntity>();
                    var callsInVsOutDto = await multi.ReadAsync<InVsOutDto>();
                    var callsPerformance = await multi.ReadAsync<PerformanceEntity>();
                    var weekOverWeekDtos = await multi.ReadAsync<WeekOverWeekEntity>();

                    return new CallMetrics
                    {
                        CallsByDayOfWeek = _entityMapper.BuildByDayOfWeek<CallByDayOfWeek>(callsByDayOfWeekDto.ToList()),
                        CallsByHour = _entityMapper.BuildByHour<CallByHour>(callsByHourDto.ToList()),
                        CallPerformance = _entityMapper.BuildByPeriod(days.ToList(),callsPerformance.ToList(), previousPeriodEndDate),
                        WeekOverWeek = _mapper.Map<List<ByWeek>>(weekOverWeekDtos),
                        TotalCalls = totalCalls,
                        TotalKnownFemaleDurationSeconds = totalKnownFemaleDuration,
                        TotalKnownMaleDurationSeconds = totalKnownMaleDuration,
                        TotalKnownDurationSeconds = totalKnownMaleDuration + totalKnownFemaleDuration,
                        TotalDurationSeconds = totalDuration,
                        TotalCallsIncoming = callsInVsOutDto.First(d => d.IsIncoming).Total,
                        TotalCallsOutgoing = callsInVsOutDto.First(d => !d.IsIncoming).Total,

                    };
                }
            }
        }
    }
}
