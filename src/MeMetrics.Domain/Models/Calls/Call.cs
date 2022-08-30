using System;
using Newtonsoft.Json;

namespace MeMetrics.Domain.Models.Calls
{
    public class Call
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string CallId { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public DateTimeOffset OccurredDate { get; set; }
        public bool IsIncoming { get; set; }
        public int Duration { get; set; }
    }
}
