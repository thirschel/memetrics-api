using System.Collections.Generic;
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

        public async Task InsertAttachments(IList<Attachment> attachments)
        {
            using (var connection = SqlConnectionBuilder.Build(_configuration.Value.DB_CONNECTION_STRING))
            {
                var sql = $@"{MergeAttachment.Value}";
                for (var i = 0; i < attachments.Count; i++)
                {
                    await connection.ExecuteAsync(sql, attachments[i]);
                }
            }
        }
    }
}
