using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using MeMetrics.Application.Commands.Call;
using MeMetrics.Application.Models;
using MeMetrics.Application.Models.Enums;
using MeMetrics.Application.Queries.Call;
using MeMetrics.Domain.Models.Calls;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MeMetrics.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/calls")]
    public class CallController : BaseController
    {

        public CallController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [Route("")]
        [Authorize]
        [HttpPost]
        [ApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<bool>> SaveCall([FromBody] Call call)
        {
            var createCallCommand = new CreateCallCommand()
            {
                Call = call,
            };
            var result = await _mediator.Send(createCallCommand);

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
        public async Task<ActionResult<CallMetrics>> GetCallMetrics([FromQuery] PeriodSelectionEnum selectedPeriod)
        {
            _logger.Information("test");
            var getMessageMetricsByPeriodSelectionQuery = new GetCallMetricsByPeriodSelectionQuery()
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
