using MediatR;
using MeMetrics.Application.Models;

namespace MeMetrics.Application.Commands.RecruitmentMessage
{
    public class CreateRecruitmentMessageCommand : IRequest<CommandResult<bool>>
    {
        public Domain.Models.RecruitmentMessage.RecruitmentMessage RecruitmentMessage { get; set; }
    }
}
