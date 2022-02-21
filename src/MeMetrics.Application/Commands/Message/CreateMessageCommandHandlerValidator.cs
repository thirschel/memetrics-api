using FluentValidation;

namespace MeMetrics.Application.Commands.Message
{
    public class CreateMessageCommandValidator : AbstractValidator<CreateMessagesCommand>
    {
        public CreateMessageCommandValidator()
        {
            RuleForEach(x => x.Messages).ChildRules(message =>
            {
                message.RuleFor(x => x.MessageId).NotNull();
            });
        }
    }
}
