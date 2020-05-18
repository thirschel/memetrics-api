using MeMetrics.Application.Models;
using MediatR;
using MeMetrics.Application.Models.Enums;
using MeMetrics.Domain.Models.RecruitmentMessage;

namespace MeMetrics.Application.Queries.RecruitmentMessages
{
    public class GetRecruitmentMessageMetricsByPeriodSelectionQuery : IRequest<QueryResult<RecruitmentMetrics>>
    {
        public PeriodSelectionEnum DatePeriod { get; set; }
    }
}
