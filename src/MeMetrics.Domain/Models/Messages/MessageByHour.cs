namespace MeMetrics.Domain.Models.Messages
{
    public class MessageByHour : ByHour
    {
        public int Incoming { get; set; }
        public int Outgoing { get; set; }
    }
}
