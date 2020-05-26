using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using MeMetrics.Application.Commands.Message;
using MeMetrics.Application.Models;
using MeMetrics.Application.Models.Enums;
using MeMetrics.Application.Queries.Messages;
using MeMetrics.Domain.Models.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MeMetrics.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/messages")]
    public class MessageController : BaseController
    {

        public MessageController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [Route("")]
        [Authorize]
        [HttpPost]
        [ApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<bool>> SaveMessage([FromBody] Message message)
        {
            var createMessageCommand = new CreateMessageCommand()
            {
                Message = message,
            };
            var result = await _mediator.Send(createMessageCommand);

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
        public async Task<ActionResult<MessageMetrics>> GetMessageMetrics([FromQuery] PeriodSelectionEnum selectedPeriod)
        {
            var getMessageMetricsByPeriodSelectionQuery = new GetMessageMetricsByPeriodSelectionQuery()
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
