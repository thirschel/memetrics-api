using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using MeMetrics.Application.Commands.Ride;
using MeMetrics.Application.Models;
using MeMetrics.Application.Models.Enums;
using MeMetrics.Application.Queries.Rides;
using MeMetrics.Domain.Models.Rides;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MeMetrics.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/rides")]
    public class RideController : BaseController
    {

        public RideController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }


        [Route("")]
        [Authorize]
        [HttpPost]
        [ApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<bool>> SaveRide([FromBody] Ride ride)
        {
            var createRideCommand = new CreateRideCommand()
            {
                Ride = ride,
            };
            var result = await _mediator.Send(createRideCommand);

            if (result.Type == CommandResultTypeEnum.InvalidInput)
            {
                return new BadRequestResult();
            }

            return new OkObjectResult(result.Result);
        }

        [Route("metrics")]
        [HttpGet]
        [ApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<RideMetrics>> GetRideMetrics([FromQuery] PeriodSelectionEnum selectedPeriod)
        {
            var getMessageMetricsByPeriodSelectionQuery = new GetRideMetricsByPeriodSelectionQuery()
            {
                DatePeriod = selectedPeriod,
            };
            var result = await _mediator.Send(getMessageMetricsByPeriodSelectionQuery);

            if (result.Type == QueryResultTypeEnum.NotFound)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(result.Result);
        }
    }
}
