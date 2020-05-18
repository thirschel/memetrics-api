using System.Collections.Generic;

namespace MeMetrics.Domain.Models.Messages
{
    public class MessageMetrics
    {
        public int TotalMessages { get; set; }
        public int UniqueContacts { get; set; }
        public int? TotalMessagesIncoming { get; set; }
        public int? TotalMessagesOutgoing { get; set; }
        public int? TotalMessagesFemale { get; set; }
        public int? TotalMessagesMale { get; set; }
        public int? AverageOutgoingTextLengthFemale { get; set; }
        public int? AverageOutgoingTextLengthMale { get; set; }
        public IEnumerable<MessageByDayOfWeek> MessagesByDayOfWeek { get; set; }
        public IEnumerable<MessageByHour> MessagesByHour { get; set; }
        public IEnumerable<ByPeriod> MessagePerformance { get; set; }
        public IEnumerable<ByWeek> WeekOverWeek { get; set; }
        public string PriorPeriodLabel { get; set; }
        public string CurrentPeriodLabel { get; set; }
    }
}