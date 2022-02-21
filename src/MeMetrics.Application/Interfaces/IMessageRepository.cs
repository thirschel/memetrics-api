using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MeMetrics.Domain.Models.Messages;

namespace MeMetrics.Application.Interfaces
{
    public interface IMessageRepository
    {
        Task<List<Message>> GetMessages();
        Task<int> InsertMessages(IList<Message> messages);

        Task<MessageMetrics> GetOverviewMessageMetrics(DateTime? startDate, DateTime endDate, DateTime? previousPeriodStartDate, DateTime? previousPeriodEndDate);
    }
}
