using FluentValidation;

namespace MeMetrics.Application.Commands.Ride
{
    public class CreateRideCommandValidator : AbstractValidator<CreateRideCommand>
    {
        public CreateRideCommandValidator()
        {
            RuleFor(x => x.Ride.RideId).NotNull();
        }
    }
}