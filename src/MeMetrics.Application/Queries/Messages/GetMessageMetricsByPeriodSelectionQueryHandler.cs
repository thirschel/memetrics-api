using System;
using System.Threading;
using System.Threading.Tasks;
using MeMetrics.Application.Models;
using MediatR;
using MeMetrics.Application.Helpers;
using MeMetrics.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using MeMetrics.Domain.Models.Messages;
using Serilog;

namespace MeMetrics.Application.Queries.Messages
{
    public class GetMessageMetricsByPeriodSelectionQueryHandler : IRequestHandler<GetMessageMetricsByPeriodSelectionQuery, QueryResult<MessageMetrics>>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly ILogger _logger;
        private readonly IMemoryCache _cache;

        public GetMessageMetricsByPeriodSelectionQueryHandler(
            IMessageRepository messageRepository,
            ILogger logger,
            IMemoryCache cache)
        {
            _messageRepository = messageRepository;
            _logger = logger;
            _cache = cache;
        }

        public async Task<QueryResult<MessageMetrics>> Handle(GetMessageMetricsByPeriodSelectionQuery request, CancellationToken cancellationToken)
        {

            var dateRange = DatePeriodHelper.GetDateRangeFromPeriodSelection(request.DatePeriod);
            var cacheKey = $"message-{dateRange.StartDate.ToString()}-{dateRange.EndDate.ToString()}";

            var metrics = await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(23);

                return await _messageRepository.GetOverviewMessageMetrics(dateRange.StartDate,
                    dateRange.EndDate,
                    dateRange.PreviousPeriodStartDate,
                    dateRange.PreviousPeriodEndDate);
            });

            metrics.CurrentPeriodLabel = dateRange.CurrentPeriodLabel;
            metrics.PriorPeriodLabel = dateRange.PriorPeriodLabel;

            return new QueryResult<MessageMetrics>(result: metrics, type: QueryResultTypeEnum.Success);
        }
    }
}
