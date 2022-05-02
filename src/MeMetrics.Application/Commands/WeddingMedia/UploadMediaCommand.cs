using MediatR;
using MeMetrics.Application.Models;
using Microsoft.AspNetCore.Http;

namespace MeMetrics.Application.Commands.WeddingMedia
{
    public class UploadMediaCommand : IRequest<CommandResult<bool>>
    {
        public IFormFile File { get; set; }
    }
}
