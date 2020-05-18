using Microsoft.AspNetCore.Mvc;
using MediatR;
using Serilog;

namespace MeMetrics.Api.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ILogger _logger;
        protected readonly IMediator _mediator;

        public BaseController(
            ILogger logger,
            IMediator mediator
        )
        {
            _logger = logger;
            _mediator = mediator;
        }

    }
}
