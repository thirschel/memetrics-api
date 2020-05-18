using MediatR;
using MeMetrics.Application.Models;

namespace MeMetrics.Application.Commands.Message
{
    public class CreateMessageCommand : IRequest<CommandResult<bool>>
    {
        public Domain.Models.Messages.Message Message { get; set; }
    }
}
