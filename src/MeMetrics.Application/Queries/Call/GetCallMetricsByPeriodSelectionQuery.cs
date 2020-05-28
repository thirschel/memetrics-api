using MeMetrics.Application.Models;
using MediatR;
using MeMetrics.Application.Models.Enums;
using MeMetrics.Domain.Models.Calls;

namespace MeMetrics.Application.Queries.Call
{
    public class GetCallMetricsByPeriodSelectionQuery : IRequest<QueryResult<CallMetrics>>
    {
        public PeriodSelectionEnum DatePeriod { get; set; }
        public bool RefreshCache { get; set; }
    }
}
