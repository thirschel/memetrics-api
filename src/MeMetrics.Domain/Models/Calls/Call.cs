using System;

namespace MeMetrics.Domain.Models.Calls
{
    public class Call
    {
        public string CallId { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public DateTimeOffset OccurredDate { get; set; }
        public bool IsIncoming { get; set; }
        public int Duration { get; set; }
    }
}
