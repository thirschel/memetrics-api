using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using MeMetrics.Application.Interfaces;
using MeMetrics.Application.Models;
using MeMetrics.Domain.Models;
using MeMetrics.Domain.Models.Rides;
using MeMetrics.Infrastructure.Helpers;
using MeMetrics.Infrastructure.SqlServer.Entities;
using MeMetrics.Infrastructure.SqlServer.Rides.Sql;
using MeMetrics.Infrastructure.SqlServer.Sql;
using Microsoft.Extensions.Options;

namespace MeMetrics.Infrastructure.SqlServer.Rides
{
    public class RideRepository : IRideRepository
    {
        private readonly IEntityMapper _entityMapper;
        private readonly IMapper _mapper;
        private readonly IOptions<EnvironmentConfiguration> _configuration;

        public RideRepository(
            IOptions<EnvironmentConfiguration> configuration,
            IEntityMapper entityMapper,
            IMapper mapper)
        {
            _entityMapper = entityMapper;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<int> InsertRide(Ride ride)
        {
            using (var connection = SqlConnectionBuilder.Build(_configuration.Value.DB_CONNECTION_STRING))
            {
                await connection.ExecuteAsync(MergeRide.Value, new
                {
                    ride.RideId,
                    ride.RideType,
                    ride.OriginLat,
                    ride.OriginLong,
                    ride.DestinationLat,
                    ride.DestinationLong,
                    ride.RequestDate,
                    ride.PickupDate,
                    ride.DropoffDate,
                    ride.Price,
                    ride.Distance,
                });
            }

            return 1;
        }

        public async Task<RideMetrics> GetOverviewRideMetrics(
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
                        {GetTotals.Value}
                        {GetTotalsByDayOfWeek.Value}
                        {GetTotalsByHour.Value}
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
                    var totalDto = await multi.ReadSingleAsync<RideTotalEntity>();
                    var ridesByDayOfWeekDtos = await multi.ReadAsync<PerDayOfWeekEntity>();
                    var ridesByHourDtos = await multi.ReadAsync<PerHourEntity>();
                    var ridesPerformanceDtos = await multi.ReadAsync<PerformanceEntity>();
                    var ridesWeekOverWeek = await multi.ReadAsync<WeekOverWeekEntity>();

                    return new RideMetrics()
                    {
                        RidesByDayOfWeek = _entityMapper.BuildByDayOfWeek<RideByDayOfWeek>(ridesByDayOfWeekDtos.ToList()),
                        RidesByHour = _entityMapper.BuildByHour<RideByHour>(ridesByHourDtos.ToList()),
                        RidePerformance = _entityMapper.BuildByPeriod(days.ToList(), ridesPerformanceDtos.ToList(), previousPeriodEndDate),
                        WeekOverWeek = _mapper.Map<List<ByWeek>>(ridesWeekOverWeek),
                        TotalRides = totalDto.Total,
                        AverageSecondsWaiting = totalDto.AverageSecondsWaiting,
                        TotalSecondsWaiting = totalDto.TotalSecondsWaiting,
                        AverageSecondsDriving = totalDto.AverageSecondsDriving,
                        TotalSecondsDriving = totalDto.TotalSecondsDriving,
                        ShortestRide = totalDto.ShortestRide,
                        LongestRide = totalDto.LongestRide,
                        AverageDistance = totalDto.AverageDistance,
                        TotalDistance = totalDto.TotalDistance,
                        TotalPrice = totalDto.TotalPrice,
                        AveragePrice = totalDto.AveragePrice,
                        MostExpensivePrice = totalDto.MostExpensivePrice,
                        FarthestDistance = totalDto.FarthestDistance,
                    };
                }
            }
        }
    }
}
