using FluentValidation;

namespace MeMetrics.Application.Commands.Transaction
{
    public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
    {
        public CreateTransactionCommandValidator()
        {
            RuleFor(x => x.Transactions).NotNull();
            RuleForEach(x => x.Transactions).SetValidator(new TransactionValidator());
        }
    }

    public class TransactionValidator : AbstractValidator<Domain.Models.Transactions.Transaction>
    {
        public TransactionValidator()
        {
            RuleFor(x => x.TransactionId).NotNull().NotEmpty();
        }
    }
}
