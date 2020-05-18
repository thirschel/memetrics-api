using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MeMetrics.Application.Models.Enums;
using MeMetrics.Application.Queries.Call;
using MeMetrics.Application.Queries.Messages;
using MeMetrics.Application.Queries.RecruitmentMessages;
using MeMetrics.Application.Queries.Rides;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MeMetrics.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/cache")]
    public class CacheController : BaseController
    {
        public CacheController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [Route("")]
        [Authorize]
        [HttpPost]
        [ApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        [ProducesResponseType(204)]
        public async Task StartCache()
        {
            foreach (var period in Enum.GetValues(typeof(PeriodSelectionEnum)).Cast<PeriodSelectionEnum>())
            {
                var tasks = new List<Task>()
                {
                    _mediator.Send(new GetCallMetricsByPeriodSelectionQuery() {DatePeriod = period}),
                    _mediator.Send(new GetRideMetricsByPeriodSelectionQuery() {DatePeriod = period}),
                    _mediator.Send(new GetMessageMetricsByPeriodSelectionQuery() {DatePeriod = period}),
                    _mediator.Send(new GetRecruitmentMessageMetricsByPeriodSelectionQuery() {DatePeriod = period}),
                };
                await Task.WhenAll(tasks);
            }
        }
    }
}
