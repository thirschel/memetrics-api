using System.Collections.Generic;

namespace MeMetrics.Domain.Models.RecruitmentMessage
{
    public class RecruitmentMetrics
    {
        public int TotalMessages { get; set; }
        public int TotalIncomingMessages { get; set; }
        public int TotalOutgoingMessages { get; set; }
        public int UniqueContacts { get; set; }
        public IEnumerable<RecruitmentMessageByDayOfWeek> MessagesByDayOfWeek { get; set; }
        public IEnumerable<ByPeriod> MessagePerformance { get; set; }
        public IEnumerable<ByWeek> WeekOverWeek { get; set; }
        public string CurrentPeriodLabel { get; set; }
        public string PriorPeriodLabel { get; set; }
    }
}