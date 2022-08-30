using System.Threading;
using System.Threading.Tasks;
using MeMetrics.Application.Models;
using FluentValidation;
using MediatR;
using MeMetrics.Application.Interfaces;
using Serilog;

namespace MeMetrics.Application.Commands.RecruitmentMessage
{
    public class CreateRecruitmentMessageCommandHandler : IRequestHandler<CreateRecruitmentMessageCommand, CommandResult<bool>>
    {
        private readonly IValidator<CreateRecruitmentMessageCommand> _validator;
        private readonly IRecruitmentMessageRepository _recruitmentMessageRepository;
        private readonly ILogger _logger;

        public CreateRecruitmentMessageCommandHandler(
            IRecruitmentMessageRepository recruitmentMessageRepository,
            ILogger logger,
            IValidator<CreateRecruitmentMessageCommand> validator)
        {
            _recruitmentMessageRepository = recruitmentMessageRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<CommandResult<bool>> Handle(CreateRecruitmentMessageCommand command, CancellationToken cancellationToken)
        {
            var validation = _validator.Validate(command);

            if (!validation.IsValid)
            {
                _logger.Error("Create Recruitment Messages Command produced errors on validation {Errors}", validation.ToString());
                return new CommandResult<bool>(result: false, type: CommandResultTypeEnum.InvalidInput);
            }
            var rowsAffected = await _recruitmentMessageRepository.InsertRecruitmentMessages(command.RecruitmentMessages);

            if (rowsAffected == 0)
            {
                return new CommandResult<bool>(result: false, type: CommandResultTypeEnum.UnprocessableEntity);
            }
            return new CommandResult<bool>(result: true, type: CommandResultTypeEnum.Success);
        }
    }
}
