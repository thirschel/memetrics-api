﻿using System;
using System.Collections.Generic;
using MeMetrics.Domain.Models.Attachments;

namespace MeMetrics.Domain.Models.Messages
{
    public class Message
    {
        public string MessageId { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public DateTimeOffset OccurredDate { get; set; }
        public bool IsIncoming { get; set; }
        public string Text { get; set; }
        public bool IsMedia { get; set; }
        public int TextLength => Text.Length;
        public string ThreadId { get; set; }
        public List<Attachment> Attachments { get; set; }
    }
}
