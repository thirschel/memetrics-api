using System;

namespace MeMetrics.Infrastructure.SqlServer.Entities
{
    public class WeekOverWeekEntity
    {
        public DateTime WeekOf { get; set; }
        public int Total { get; set; }
    }
}
