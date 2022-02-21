using System.Collections.Generic;
using MediatR;
using MeMetrics.Application.Models;

namespace MeMetrics.Application.Commands.Message
{
    public class CreateMessagesCommand : IRequest<CommandResult<bool>>
    {
        public IList<Domain.Models.Messages.Message> Messages { get; set; }
    }
}
