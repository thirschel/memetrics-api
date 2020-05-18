using System;

namespace MeMetrics.Domain.Models
{
    public class ByWeek
    {
        public DateTime WeekOf { get; set; }
        public int Total { get; set; }
    }
}