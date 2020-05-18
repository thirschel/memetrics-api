using System;

namespace MeMetrics.Application.Models
{
    public class DateRange
    {
        public DateTime? StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? PreviousPeriodStartDate { get; set; }
        public DateTime? PreviousPeriodEndDate { get; set; }
        public string PriorPeriodLabel { get; set; }
        public string CurrentPeriodLabel { get; set; }
    }
}
