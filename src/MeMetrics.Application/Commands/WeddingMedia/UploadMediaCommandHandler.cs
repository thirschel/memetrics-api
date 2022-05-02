using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using MediatR;
using MeMetrics.Application.Interfaces;
using MeMetrics.Application.Models;
using Microsoft.Extensions.Options;
using Serilog;

namespace MeMetrics.Application.Commands.WeddingMedia
{
    public class UploadMediaCommandHandler : IRequestHandler<UploadMediaCommand, CommandResult<bool>>
    {
        private readonly IOptions<EnvironmentConfiguration> _configuration;
        private readonly ILogger _logger;
        private readonly IGooglePhotoUploader _googlePhotoUploader;

        public UploadMediaCommandHandler(
            IOptions<EnvironmentConfiguration> configuration,
            ILogger logger,
            IGooglePhotoUploader googlePhotoUploader)
        {
            _configuration = configuration;
            _logger = logger;
            _googlePhotoUploader = googlePhotoUploader;
        }

        public async Task<CommandResult<bool>> Handle(UploadMediaCommand command, CancellationToken cancellationToken)
        {
            var wasUploadSuccessful = await _googlePhotoUploader.UploadMedia(command.File);
            if (!wasUploadSuccessful)
            {
                _logger.Warning("Upload to Google photos failed. Falling back to saving to blob storage.");
                try
                {
                    var blobServiceClient = new BlobServiceClient(_configuration.Value.BLOB_STORAGE_CONNECTION_STRING);
                    var containerName = "media";
                    var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                    var blobClient = containerClient.GetBlobClient(command.File.FileName);
                    using (var stream = command.File.OpenReadStream())
                    {
                        await blobClient.UploadAsync(stream, true);
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e.Message, e);
                    return new CommandResult<bool>() { Result = false, Type = CommandResultTypeEnum.UnprocessableEntity };
                }
            }

            return new CommandResult<bool>(){ Result = true, Type = CommandResultTypeEnum.Success};
        }
    }
}
