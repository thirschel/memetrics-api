using System.Collections.Generic;

namespace MeMetrics.Domain.Models.Calls
{
    public class CallMetrics
    {
        public int TotalCalls { get; set; }
        public int TotalCallsIncoming { get; set; }
        public int TotalCallsOutgoing { get; set; }
        public int TotalDurationSeconds { get; set; }
        public int TotalKnownDurationSeconds { get; set; }
        public int TotalKnownMaleDurationSeconds { get; set; }
        public int TotalKnownFemaleDurationSeconds { get; set; }
        public IEnumerable<CallByDayOfWeek> CallsByDayOfWeek { get; set; }
        public IEnumerable<CallByHour> CallsByHour { get; set; }
        public IEnumerable<ByPeriod> CallPerformance { get; set; }
        public IEnumerable<ByWeek> WeekOverWeek { get; set; }
        public string CurrentPeriodLabel { get; set; }
        public string PriorPeriodLabel { get; set; }
    }
}
