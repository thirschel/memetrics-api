using FluentValidation;

namespace MeMetrics.Application.Commands.Call
{
    public class CreateCallCommandValidator : AbstractValidator<CreateCallCommand>
    {
        public CreateCallCommandValidator()
        {
            RuleFor(x => x.Call).NotNull();
        }
    }
}
