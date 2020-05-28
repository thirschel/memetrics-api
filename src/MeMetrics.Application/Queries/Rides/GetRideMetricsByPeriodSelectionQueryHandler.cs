using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MeMetrics.Application.Helpers;
using MeMetrics.Application.Interfaces;
using MeMetrics.Application.Models;
using MeMetrics.Domain.Models.Rides;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

namespace MeMetrics.Application.Queries.Rides
{
    public class GetRideMetricsByPeriodSelectionQueryHandler : IRequestHandler<GetRideMetricsByPeriodSelectionQuery, QueryResult<RideMetrics>>
    {
        private readonly IRideRepository _rideRepository;
        private readonly ILogger _logger;
        private readonly IMemoryCache _cache;

        public GetRideMetricsByPeriodSelectionQueryHandler(
            IRideRepository rideRepository,
            ILogger logger,
            IMemoryCache cache)
        {
            _rideRepository = rideRepository;
            _logger = logger;
            _cache = cache;
        }

        public async Task<QueryResult<RideMetrics>> Handle(GetRideMetricsByPeriodSelectionQuery request, CancellationToken cancellationToken)
        {

            var dateRange = DatePeriodHelper.GetDateRangeFromPeriodSelection(request.DatePeriod);
            var cacheKey = $"ride-{dateRange.StartDate.ToString()}-{dateRange.EndDate.ToString()}";

            if (request.RefreshCache)
            {
                _cache.Remove(cacheKey);
            }

            var metrics = await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(23);

                return await _rideRepository.GetOverviewRideMetrics(dateRange.StartDate,
                    dateRange.EndDate,
                    dateRange.PreviousPeriodStartDate,
                    dateRange.PreviousPeriodEndDate);
            });

            metrics.CurrentPeriodLabel = dateRange.CurrentPeriodLabel;
            metrics.PriorPeriodLabel = dateRange.PriorPeriodLabel;

            return new QueryResult<RideMetrics>(result: metrics, type: QueryResultTypeEnum.Success);
        }
    }
}
