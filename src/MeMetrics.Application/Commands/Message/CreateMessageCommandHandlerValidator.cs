using FluentValidation;

namespace MeMetrics.Application.Commands.Message
{
    public class CreateMessageCommandValidator : AbstractValidator<CreateMessageCommand>
    {
        public CreateMessageCommandValidator()
        {
            RuleFor(x => x.Message.MessageId).NotNull();
        }
    }
}
