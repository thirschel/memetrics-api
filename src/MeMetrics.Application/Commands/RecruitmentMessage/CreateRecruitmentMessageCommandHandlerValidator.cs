using FluentValidation;

namespace MeMetrics.Application.Commands.RecruitmentMessage
{
    public class CreateRecruitmentMessageCommandValidator : AbstractValidator<CreateRecruitmentMessageCommand>
    {
        public CreateRecruitmentMessageCommandValidator()
        {
            RuleFor(x => x.RecruitmentMessages).NotNull();
            RuleForEach(x => x.RecruitmentMessages).SetValidator(new RecruitmentMessageValidator());
        }
    }

    public class RecruitmentMessageValidator : AbstractValidator<Domain.Models.RecruitmentMessage.RecruitmentMessage>
    {
        public RecruitmentMessageValidator()
        {
            RuleFor(x => x.RecruiterId).NotNull().NotEmpty();
        }
    }
}
