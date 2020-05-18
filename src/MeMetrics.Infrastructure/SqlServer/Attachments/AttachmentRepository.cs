using System.Threading.Tasks;
using Dapper;
using MeMetrics.Domain.Models.Attachments;
using MeMetrics.Infrastructure.Helpers;
using MeMetrics.Infrastructure.SqlServer.Attachments.Sql;
using Microsoft.Extensions.Options;

namespace MeMetrics.Infrastructure.SqlServer.Attachments
{
    public class AttachmentRepository : Application.Interfaces.IAttachmentRepository
    {
        private readonly IOptions<EnvironmentConfiguration> _configuration;

        public AttachmentRepository(
            IOptions<EnvironmentConfiguration> configuration)
        {
            _configuration = configuration;
        }

        public async Task InsertAttachment(Attachment attachment, string messageId)
        {
            using (var connection = SqlConnectionBuilder.Build(_configuration.Value.DB_CONNECTION_STRING))
            {
                var sql = $@"{MergeAttachment.Value}";
                await connection.ExecuteAsync(sql, new
                {
                    attachment.AttachmentId,
                    MessageId = messageId,
                    attachment.Base64Data,
                    attachment.FileName,
                });
            }
        }
    }
}
