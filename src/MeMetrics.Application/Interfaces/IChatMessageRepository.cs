using System.Collections.Generic;
using System.Threading.Tasks;
using MeMetrics.Domain.Models;

namespace MeMetrics.Application.Interfaces
{
    public interface IChatMessageRepository
    {
        Task<List<ChatMessage>> GetChatMessages();
        Task<int> InsertChatMessage(ChatMessage chatMessage);
    }
}
