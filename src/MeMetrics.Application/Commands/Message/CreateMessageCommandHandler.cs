using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MeMetrics.Application.Models;
using FluentValidation;
using MediatR;
using MeMetrics.Application.Commands.Call;
using MeMetrics.Application.Interfaces;
using MeMetrics.Domain.Models.Attachments;
using Serilog;

namespace MeMetrics.Application.Commands.Message
{
    public class CreateMessageCommandHandler : IRequestHandler<CreateMessagesCommand, CommandResult<bool>>
    {
        private readonly IValidator<CreateMessagesCommand> _validator;
        private readonly IMessageRepository _messageRepository;
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly ILogger _logger;

        public CreateMessageCommandHandler(
            IMessageRepository messageRepository,
            IAttachmentRepository attachmentRepository,
            ILogger logger,
            IValidator<CreateMessagesCommand> validator)
        {
            _messageRepository = messageRepository;
            _attachmentRepository = attachmentRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<CommandResult<bool>> Handle(CreateMessagesCommand command, CancellationToken cancellationToken)
        {
            var validation = _validator.Validate(command);

            if (!validation.IsValid)
            {
                _logger.Error("Create Messages Command produced errors on validation {Errors}", validation.ToString());
                return new CommandResult<bool>(result: false, type: CommandResultTypeEnum.InvalidInput);
            }
            var insertMessageTask = _messageRepository.InsertMessages(command.Messages);

            var attachments = command.Messages.SelectMany(x =>
            {
                x.Attachments?.ForEach(y => y.MessageId = x.MessageId);
                return x.Attachments ?? new List<Attachment>();
            }).ToList();

            if (attachments.Any())
            {
                await _attachmentRepository.InsertAttachments(attachments);
            }

            var rowsAffected = await insertMessageTask;
            if (rowsAffected == 0)
            {
                return new CommandResult<bool>(result: false, type: CommandResultTypeEnum.UnprocessableEntity);
            }
            return new CommandResult<bool>(result: true, type: CommandResultTypeEnum.Success);
        }
    }
}
