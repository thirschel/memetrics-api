namespace MeMetrics.Infrastructure.SqlServer.Entities
{
    public class PerHourEntity
    {
        public int? Total { get; set; }
        public int? IncomingTotal { get; set; }
        public int? OutgoingTotal { get; set; }
        public bool IsIncoming { get; set; }
        public int Hour { get; set; }
        public int Day { get; set; }

    }
}
