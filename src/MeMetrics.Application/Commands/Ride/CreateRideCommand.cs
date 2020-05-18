using MeMetrics.Application.Models;
using MediatR;

namespace MeMetrics.Application.Commands.Ride
{
    public class CreateRideCommand : IRequest<CommandResult<bool>>
    {
        public Domain.Models.Rides.Ride Ride { get; set; }
    }
}
