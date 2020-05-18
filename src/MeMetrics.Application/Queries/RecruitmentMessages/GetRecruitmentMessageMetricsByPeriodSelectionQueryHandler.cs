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
    public class GetRideMetricsByPeriodSelectionQueryHandler : IRequestHandler<GetRecruitmentMessageMetricsByPeriodSelectionQuery, QueryResult<RecruitmentMetrics>>
    {
        private readonly IRecruitmentMessageRepository _recruitmentMessageRepository;
        private readonly ILogger _logger;
        private readonly IMemoryCache _cache;

        public GetRideMetricsByPeriodSelectionQueryHandler(
            IRecruitmentMessageRepository recruitmentMessageRepository,
            ILogger logger,
            IMemoryCache cache)
        {
            _recruitmentMessageRepository = recruitmentMessageRepository;
            _logger = logger;
            _cache = cache;
        }

        public async Task<QueryResult<RecruitmentMetrics>> Handle(GetRecruitmentMessageMetricsByPeriodSelectionQuery request, CancellationToken cancellationToken)
        {

            var dateRange = DatePeriodHelper.GetDateRangeFromPeriodSelection(request.DatePeriod);
            var cacheKey = $"recruitment-message-{dateRange.StartDate.ToString()}-{dateRange.EndDate.ToString()}";

            var metrics = await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(23);

                return await _recruitmentMessageRepository.GetRecruiterMetrics(dateRange.StartDate,
                    dateRange.EndDate,
                    dateRange.PreviousPeriodStartDate,
                    dateRange.PreviousPeriodEndDate);
            });

            metrics.CurrentPeriodLabel = dateRange.CurrentPeriodLabel;
            metrics.PriorPeriodLabel = dateRange.PriorPeriodLabel;

            return new QueryResult<RecruitmentMetrics>(result: metrics, type: QueryResultTypeEnum.Success);
        }
    }
}
