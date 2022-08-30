using System.Collections.Generic;
using MediatR;
using MeMetrics.Application.Models;

namespace MeMetrics.Application.Commands.RecruitmentMessage
{
    public class CreateRecruitmentMessageCommand : IRequest<CommandResult<bool>>
    {
        public List<Domain.Models.RecruitmentMessage.RecruitmentMessage> RecruitmentMessages { get; set; }
    }
}
