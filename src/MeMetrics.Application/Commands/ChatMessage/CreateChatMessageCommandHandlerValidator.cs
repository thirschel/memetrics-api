using FluentValidation;

namespace MeMetrics.Application.Commands.ChatMessage
{
    public class CreateChatMessageCommandValidator : AbstractValidator<CreateChatMessageCommand>
    {
        public CreateChatMessageCommandValidator()
        {
            RuleFor(x => x.ChatMessage.ChatMessageId).NotNull();
        }
    }
}
