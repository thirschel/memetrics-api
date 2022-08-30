using MeMetrics.Application.Models;
using MediatR;
using System.Collections.Generic;

namespace MeMetrics.Application.Commands.Call
{
    public class CreateCallCommand : IRequest<CommandResult<bool>>
    {
        public List<Domain.Models.Calls.Call> Calls { get; set; }
    }
}
