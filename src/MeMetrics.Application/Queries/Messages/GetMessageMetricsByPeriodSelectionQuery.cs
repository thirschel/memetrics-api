using MeMetrics.Application.Models;
using MediatR;
using MeMetrics.Application.Models.Enums;
using MeMetrics.Domain.Models.Messages;

namespace MeMetrics.Application.Queries.Messages
{
    public class GetMessageMetricsByPeriodSelectionQuery : IRequest<QueryResult<MessageMetrics>>
    {
        public PeriodSelectionEnum DatePeriod { get; set; }
        public bool RefreshCache { get; set; }
    }
}
