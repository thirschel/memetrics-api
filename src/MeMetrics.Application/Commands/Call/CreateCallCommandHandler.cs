using System.Threading;
using System.Threading.Tasks;
using MeMetrics.Application.Models;
using FluentValidation;
using MediatR;
using MeMetrics.Application.Interfaces;
using Serilog;

namespace MeMetrics.Application.Commands.Call
{
    public class CreateCallCommandHandler : IRequestHandler<CreateCallCommand, CommandResult<bool>>
    {
        private readonly IValidator<CreateCallCommand> _validator;
        private readonly ICallRepository _callRepository;
        private readonly ILogger _logger;

        public CreateCallCommandHandler(
            ICallRepository callRepository,
            ILogger logger,
            IValidator<CreateCallCommand> validator)
        {
            _callRepository = callRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<CommandResult<bool>> Handle(CreateCallCommand command, CancellationToken cancellationToken)
        {
            var validation = _validator.Validate(command);

            if (!validation.IsValid)
            {
                _logger.Error("Create Call Command produced errors on validation {Errors}", validation.ToString());
                return new CommandResult<bool>(result: false, type: CommandResultTypeEnum.InvalidInput);
            }
            var rowsAffected = await _callRepository.InsertCalls(command.Calls);

            if (rowsAffected == 0)
            {
                return new CommandResult<bool>(result: false, type: CommandResultTypeEnum.UnprocessableEntity);
            }
            return new CommandResult<bool>(result: true, type: CommandResultTypeEnum.Success);
        }
    }
}
