using System;

namespace MeMetrics.Infrastructure.SqlServer.Entities
{
    public class PerformanceEntity
    {
        public DateTime OccurredDate { get; set; }
        public int Count { get; set; }
        public bool IsPreviousPeriod { get; set; }
    }
}
