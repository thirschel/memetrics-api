namespace MeMetrics.Infrastructure.SqlServer.Messages
{
    public class MessageByGenderEntity
    {
        public int Total { get; set; }
        public string Gender { get; set; }
        public bool IsIncoming { get; set; }
        public int AverageTextLength { get; set; }
    }
}
