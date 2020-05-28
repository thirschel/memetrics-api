using System;
using System.Threading;
using System.Threading.Tasks;
using MeMetrics.Application.Models;
using MediatR;
using MeMetrics.Application.Helpers;
using Microsoft.Extensions.Caching.Memory;
using MeMetrics.Application.Interfaces;
using MeMetrics.Domain.Models.Calls;
using Serilog;

namespace MeMetrics.Application.Queries.Call
{
    public class GetCallMetricsByPeriodSelectionQueryHandler : IRequestHandler<GetCallMetricsByPeriodSelectionQuery, QueryResult<CallMetrics>>
    {
        private readonly ICallRepository _callRepository;
        private readonly ILogger _logger;
        private readonly IMemoryCache _cache;

        public GetCallMetricsByPeriodSelectionQueryHandler(
            ICallRepository callRepository,
            ILogger logger,
            IMemoryCache cache)
        {
            _callRepository = callRepository;
            _logger = logger;
            _cache = cache;
        }

        public async Task<QueryResult<CallMetrics>> Handle(GetCallMetricsByPeriodSelectionQuery request, CancellationToken cancellationToken)
        {

            var dateRange = DatePeriodHelper.GetDateRangeFromPeriodSelection(request.DatePeriod);
            var cacheKey = $"call-{dateRange.StartDate.ToString()}-{dateRange.EndDate.ToString()}";

            if (request.RefreshCache)
            {
                _cache.Remove(cacheKey);
            }

            var metrics = await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(23);

                return await _callRepository.GetOverviewCallMetrics(dateRange.StartDate,
                    dateRange.EndDate,
                    dateRange.PreviousPeriodStartDate,
                    dateRange.PreviousPeriodEndDate);
            });

            metrics.CurrentPeriodLabel = dateRange.CurrentPeriodLabel;
            metrics.PriorPeriodLabel = dateRange.PriorPeriodLabel;

            return new QueryResult<CallMetrics>(result: metrics, type: QueryResultTypeEnum.Success);
        }
    }
}
