namespace MeMetrics.Domain.Models.Calls
{
    public class CallByDayOfWeek : ByDayOfWeek
    {
        public int Incoming { get; set; }
        public int Outgoing { get; set; }
    }
}
