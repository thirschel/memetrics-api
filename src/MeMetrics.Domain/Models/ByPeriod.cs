namespace MeMetrics.Domain.Models
{
    public class ByPeriod
    {
        public int DayNumber { get; set; }
        public int Count { get; set; }
        public bool IsPreviousPeriod { get; set; }
    }
}