using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MeMetrics.Application.Interfaces;
using MeMetrics.Application.Models;
using Serilog;

namespace MeMetrics.Application.Commands.ChatMessage
{
    public class CreateChatMessageCommandHandler : IRequestHandler<CreateChatMessageCommand, CommandResult<bool>>
    {
        private readonly IValidator<CreateChatMessageCommand> _validator;
        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly ILogger _logger;

        public CreateChatMessageCommandHandler(
            IChatMessageRepository chatMessageRepository,
            ILogger logger,
            IValidator<CreateChatMessageCommand> validator)
        {
            _chatMessageRepository = chatMessageRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<CommandResult<bool>> Handle(CreateChatMessageCommand command, CancellationToken cancellationToken)
        {
            var validation = _validator.Validate(command);

            if (!validation.IsValid)
            {
                _logger.Error("Create Ride Command produced errors on validation {Errors}", validation.ToString());
                return new CommandResult<bool>(result: false, type: CommandResultTypeEnum.InvalidInput);
            }
            var rowsAffected = await _chatMessageRepository.InsertChatMessage(command.ChatMessage);

            if (rowsAffected == 0)
            {
                return new CommandResult<bool>(result: false, type: CommandResultTypeEnum.UnprocessableEntity);
            }
            return new CommandResult<bool>(result: true, type: CommandResultTypeEnum.Success);
        }
    }
}
