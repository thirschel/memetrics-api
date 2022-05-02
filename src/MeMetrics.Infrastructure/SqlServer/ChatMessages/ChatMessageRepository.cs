using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MeMetrics.Application.Interfaces;
using MeMetrics.Application.Models;
using MeMetrics.Domain.Models;
using MeMetrics.Infrastructure.Helpers;
using MeMetrics.Infrastructure.SqlServer.ChatMessages.Sql;
using Microsoft.Extensions.Options;

namespace MeMetrics.Infrastructure.SqlServer.ChatMessages
{
    public class CallRepository : IChatMessageRepository
    {
        private readonly IOptions<EnvironmentConfiguration> _configuration;

        public CallRepository(
            IOptions<EnvironmentConfiguration> configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<ChatMessage>> GetChatMessages()
        {
            using (var connection = SqlConnectionBuilder.Build(_configuration.Value.DB_CONNECTION_STRING))
            {
                var calls = await connection.QueryAsync<ChatMessage>(Sql.GetChatMessages.Value);
                return calls.ToList();
            }
        }

        public async Task<int> InsertChatMessage(ChatMessage message)
        {
            using (var connection = SqlConnectionBuilder.Build(_configuration.Value.DB_CONNECTION_STRING))
            {
                await connection.ExecuteAsync(MergeChatMessages.Value, new
                {
                    message.ChatMessageId,
                    message.GroupId,
                    message.GroupName,
                    message.SenderName,
                    message.SenderId,
                    message.OccurredDate,
                    message.IsIncoming,
                    message.Text,
                    message.IsMedia,
                    message.TextLength,
                });

            }

            return 1;
        }
    }
}
