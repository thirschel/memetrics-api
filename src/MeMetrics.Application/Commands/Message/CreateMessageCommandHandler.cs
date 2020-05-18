using System.Threading;
using System.Threading.Tasks;
using MeMetrics.Application.Models;
using FluentValidation;
using MediatR;
using MeMetrics.Application.Commands.Call;
using MeMetrics.Application.Interfaces;
using Serilog;

namespace MeMetrics.Application.Commands.Message
{
    public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, CommandResult<bool>>
    {
        private readonly IValidator<CreateMessageCommand> _validator;
        private readonly IMessageRepository _messageRepository;
        private readonly ILogger _logger;

        public CreateMessageCommandHandler(
            IMessageRepository messageRepository,
            ILogger logger,
            IValidator<CreateMessageCommand> validator)
        {
            _messageRepository = messageRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<CommandResult<bool>> Handle(CreateMessageCommand command, CancellationToken cancellationToken)
        {
            var validation = _validator.Validate(command);

            if (!validation.IsValid)
            {
                _logger.Error("Create Message Command produced errors on validation {Errors}", validation.ToString());
                return new CommandResult<bool>(result: false, type: CommandResultTypeEnum.InvalidInput);
            }
            var rowsAffected = await _messageRepository.InsertMessage(command.Message);

            if (rowsAffected == 0)
            {
                return new CommandResult<bool>(result: false, type: CommandResultTypeEnum.UnprocessableEntity);
            }
            return new CommandResult<bool>(result: true, type: CommandResultTypeEnum.Success);
        }
    }
}
