using MeMetrics.Application.Models;
using MediatR;
using MeMetrics.Application.Models.Enums;
using MeMetrics.Domain.Models.Rides;

namespace MeMetrics.Application.Queries.Rides
{
    public class GetRideMetricsByPeriodSelectionQuery : IRequest<QueryResult<RideMetrics>>
    {
        public PeriodSelectionEnum DatePeriod { get; set; }
    }
}
