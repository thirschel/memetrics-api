using FluentValidation;

namespace MeMetrics.Application.Commands.RecruitmentMessage
{
    public class CreateRecruitmentMessageCommandValidator : AbstractValidator<CreateRecruitmentMessageCommand>
    {
        public CreateRecruitmentMessageCommandValidator()
        {
            RuleFor(x => x.RecruitmentMessages).NotNull();
        }
    }
}
