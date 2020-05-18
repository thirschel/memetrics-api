using System;

namespace MeMetrics.Domain.Models.ChatMessages
{
    public class ChatMessage
    {
        public string ChatMessageId { get; set; }

        public string GroupId { get; set; }

        public string GroupName { get; set; }

        public string SenderName { get; set; }

        public string SenderId { get; set; }

        public DateTime OccurredDate { get; set; }

        public bool IsIncoming { get; set; }

        public string Text { get; set; }

        public bool IsMedia { get; set; }

        public int TextLength { get; set; }

    }
}