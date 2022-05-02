using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MediatR;
using MeMetrics.Application.Commands.Transaction;
using MeMetrics.Application.Commands.WeddingMedia;
using MeMetrics.Application.Models;
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
    [Route("api/v{version:apiVersion}/weddingmedia")]
    public class WeddingMediaController : BaseController
    {
        public WeddingMediaController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [Route("")]
        [HttpPost]
        [ApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        [ProducesResponseType(204)]
        public async Task<ActionResult<bool>> UploadMedia()
        {
            try
            {
                var file = Request.Form.Files[0];
                /*var folderName = Path.Combine("");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    Console.WriteLine(dbPath);
                }*/
                var uploadMediaCommand = new UploadMediaCommand()
                {
                    File = file,
                };
                var result = await _mediator.Send(uploadMediaCommand);

                if (result.Type == CommandResultTypeEnum.InvalidInput)
                {
                    return new BadRequestResult();
                }

                return new OkObjectResult(result.Result);
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to upload media", ex);
                return false;
            }
        }
    }
}
