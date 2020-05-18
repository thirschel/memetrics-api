namespace MeMetrics.Domain.Models.Messages
{
    public class MessageByDayOfWeek : ByDayOfWeek
    {
        public int Incoming { get; set; }
        public int Outgoing { get; set; }
    }
}
