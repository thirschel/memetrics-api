using MediatR;
using MeMetrics.Application.Models;

namespace MeMetrics.Application.Commands.ChatMessage
{
    public class CreateChatMessageCommand : IRequest<CommandResult<bool>>
    {
        public Domain.Models.ChatMessage ChatMessage { get; set; }
    }
}
