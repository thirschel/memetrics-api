using MeMetrics.Application.Models.Enums;

namespace MeMetrics.Infrastructure.SqlServer.Entities
{
    public class PerDayOfWeekEntity
    {
        public int Total { get; set; }
        public int? IncomingTotal { get; set; }
        public int? OutgoingTotal { get; set; }
        public bool IsIncoming { get; set; }
        public DayOfWeekEnum DayOfWeek { get; set; }
    }
}
