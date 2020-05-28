using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MeMetrics.Application.Models;
using MediatR;
using MeMetrics.Application.Helpers;
using MeMetrics.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using MeMetrics.Domain.Models.RecruitmentMessage;
using Serilog;

namespace MeMetrics.Application.Queries.RecruitmentMessages
{
    public class GetRecruitmentMessageMetricsByPeriodSelectionQueryHandler : IRequestHandler<GetRecruitmentMessageMetricsByPeriodSelectionQuery, QueryResult<RecruitmentMessageMetrics>>
    {
        private readonly IRecruitmentMessageRepository _recruitmentMessageRepository;
        private readonly ILogger _logger;
        private readonly IMemoryCache _cache;

        public GetRecruitmentMessageMetricsByPeriodSelectionQueryHandler(
            IRecruitmentMessageRepository recruitmentMessageRepository,
            ILogger logger,
            IMemoryCache cache)
        {
            _recruitmentMessageRepository = recruitmentMessageRepository;
            _logger = logger;
            _cache = cache;
        }

        public async Task<QueryResult<RecruitmentMessageMetrics>> Handle(GetRecruitmentMessageMetricsByPeriodSelectionQuery request, CancellationToken cancellationToken)
        {

            var dateRange = DatePeriodHelper.GetDateRangeFromPeriodSelection(request.DatePeriod);
            var cacheKey = $"recruitment-message-{dateRange.StartDate.ToString()}-{dateRange.EndDate.ToString()}";

            if (request.RefreshCache)
            {
                _cache.Remove(cacheKey);
            }

            var metrics = await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(23);

                return await _recruitmentMessageRepository.GetOverviewRecruitmentMessageMetrics(dateRange.StartDate,
                    dateRange.EndDate,
                    dateRange.PreviousPeriodStartDate,
                    dateRange.PreviousPeriodEndDate);
            });

            metrics.CurrentPeriodLabel = dateRange.CurrentPeriodLabel;
            metrics.PriorPeriodLabel = dateRange.PriorPeriodLabel;

            return new QueryResult<RecruitmentMessageMetrics>(result: metrics, type: QueryResultTypeEnum.Success);
        }
    }
}
