using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using MeMetrics.Application.Commands.Ride;
using MeMetrics.Application.Commands.Transaction;
using MeMetrics.Application.Models;
using MeMetrics.Domain.Models.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MeMetrics.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/transactions")]
    public class TransactionController : BaseController
    {

        public TransactionController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [Route("")]
        [Authorize]
        [HttpPost]
        [ApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<bool>> SaveTransaction([FromBody] Transaction transaction)
        {
            var createTransactionCommand = new CreateTransactionCommand()
            {
                Transaction = transaction,
            };
            var result = await _mediator.Send(createTransactionCommand);

            if (result.Type == CommandResultTypeEnum.InvalidInput)
            {
                return new BadRequestResult();
            }

            return new OkObjectResult(result.Result);
        }
    }
}
