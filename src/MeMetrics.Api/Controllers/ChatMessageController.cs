using System.Threading.Tasks;
using MediatR;
using MeMetrics.Application.Commands.ChatMessage;
using MeMetrics.Application.Models;
using MeMetrics.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MeMetrics.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/chat-messages")]
    public class ChatMessageController : BaseController
    {

        public ChatMessageController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [Route("")]
        [Authorize]
        [HttpPost]
        [ApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<bool>> SaveChatMessage([FromBody] ChatMessage message)
        {
            var createMessageCommand = new CreateChatMessageCommand()
            {
                ChatMessage = message,
            };
            var result = await _mediator.Send(createMessageCommand);

            if (result.Type == CommandResultTypeEnum.InvalidInput)
            {
                return new BadRequestResult();
            }

            return new OkObjectResult(result.Result);
        }
    }
}
