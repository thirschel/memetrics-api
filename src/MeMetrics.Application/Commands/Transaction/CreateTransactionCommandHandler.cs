using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MeMetrics.Application.Interfaces;
using MeMetrics.Application.Models;
using Serilog;

namespace MeMetrics.Application.Commands.Transaction
{
    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, CommandResult<bool>>
    {
        private readonly IValidator<CreateTransactionCommand> _validator;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger _logger;

        public CreateTransactionCommandHandler(
            ITransactionRepository transactionRepository,
            ILogger logger,
            IValidator<CreateTransactionCommand> validator)
        {
            _transactionRepository = transactionRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<CommandResult<bool>> Handle(CreateTransactionCommand command, CancellationToken cancellationToken)
        {
            var validation = _validator.Validate(command);

            if (!validation.IsValid)
            {
                _logger.Error("Create Ride Command produced errors on validation {Errors}", validation.ToString());
                return new CommandResult<bool>(result: false, type: CommandResultTypeEnum.InvalidInput);
            }
            var rowsAffected = await _transactionRepository.InsertTransactions(command.Transactions);

            if (rowsAffected == 0)
            {
                return new CommandResult<bool>(result: false, type: CommandResultTypeEnum.UnprocessableEntity);
            }
            return new CommandResult<bool>(result: true, type: CommandResultTypeEnum.Success);
        }
    }
}
