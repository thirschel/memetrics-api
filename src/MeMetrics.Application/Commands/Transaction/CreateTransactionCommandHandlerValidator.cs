using FluentValidation;

namespace MeMetrics.Application.Commands.Transaction
{
    public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
    {
        public CreateTransactionCommandValidator()
        {
            RuleFor(x => x.Transaction.TransactionId).NotNull();
        }
    }
}
