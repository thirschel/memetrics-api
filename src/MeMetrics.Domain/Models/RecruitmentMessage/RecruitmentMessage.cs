﻿using System;

namespace MeMetrics.Domain.Models.RecruitmentMessage
{
    public class RecruitmentMessage
    {
        public string RecruitmentMessageId { get; set; }

        public RecruitmentMessageSource MessageSource { get; set; }

        public string RecruiterId { get; set; }

        public string RecruiterName { get; set; }

        public string RecruiterCompany { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public bool IsIncoming { get; set; }

        public DateTime OccurredDate { get; set; }
    }
}