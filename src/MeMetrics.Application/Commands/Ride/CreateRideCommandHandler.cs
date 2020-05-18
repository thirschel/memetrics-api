using System.Threading;
using System.Threading.Tasks;
using MeMetrics.Application.Models;
using FluentValidation;
using MediatR;
using MeMetrics.Application.Interfaces;
using Serilog;

namespace MeMetrics.Application.Commands.Ride
{
    public class CreateRideCommandHandler : IRequestHandler<CreateRideCommand, CommandResult<bool>>
    {
        private readonly IValidator<CreateRideCommand> _validator;
        private readonly IRideRepository _rideRepository;
        private readonly ILogger _logger;

        public CreateRideCommandHandler(
            IRideRepository rideRepository,
            ILogger logger,
            IValidator<CreateRideCommand> validator)
        {
            _rideRepository = rideRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<CommandResult<bool>> Handle(CreateRideCommand command, CancellationToken cancellationToken)
        {
            var validation = _validator.Validate(command);

            if (!validation.IsValid)
            {
                _logger.Error("Create Ride Command produced errors on validation {Errors}", validation.ToString());
                return new CommandResult<bool>(result: false, type: CommandResultTypeEnum.InvalidInput);
            }
            var rowsAffected = await _rideRepository.InsertRide(command.Ride);

            if (rowsAffected == 0)
            {
                return new CommandResult<bool>(result: false, type: CommandResultTypeEnum.UnprocessableEntity);
            }
            return new CommandResult<bool>(result: true, type: CommandResultTypeEnum.Success);
        }
    }
}
