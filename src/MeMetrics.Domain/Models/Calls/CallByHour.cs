namespace MeMetrics.Domain.Models.Calls
{
    public class CallByHour : ByHour
    {
        public int Incoming { get; set; }
        public int Outgoing { get; set; }
    }
}
