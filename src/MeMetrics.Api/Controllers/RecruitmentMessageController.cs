using System.Threading.Tasks;
using MediatR;
using MeMetrics.Application.Commands.RecruitmentMessage;
using MeMetrics.Application.Models;
using MeMetrics.Application.Models.Enums;
using MeMetrics.Application.Queries.RecruitmentMessages;
using MeMetrics.Domain.Models.RecruitmentMessage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MeMetrics.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/recruitment-messages")]
    public class RecruitmentMessageController : BaseController
    {

        public RecruitmentMessageController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [Route("")]
        [Authorize]
        [HttpPost]
        [ApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<bool>> SaveRecruitmentMessage([FromBody] RecruitmentMessage message)
        {
            var createRecruitmentMessageCommand = new CreateRecruitmentMessageCommand()
            {
                RecruitmentMessage = message,
            };
            var result = await _mediator.Send(createRecruitmentMessageCommand);

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
        public async Task<ActionResult<RecruitmentMessageMetrics>> GetRecruiterMetrics([FromQuery] PeriodSelectionEnum selectedPeriod)
        {
            var getMessageMetricsByPeriodSelectionQuery = new GetRecruitmentMessageMetricsByPeriodSelectionQuery()
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
