using MeMetrics.Application.Models;
using MediatR;

namespace MeMetrics.Application.Commands.Call
{
    public class CreateCallCommand : IRequest<CommandResult<bool>>
    {
        public Domain.Models.Calls.Call Call { get; set; }
    }
}
