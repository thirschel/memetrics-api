namespace MeMetrics.Infrastructure.SqlServer.Rides
{
    public class RideTotalEntity
    {
        public int Total { get; set; }
        public int AverageSecondsWaiting { get; set; }
        public int TotalSecondsWaiting { get; set; }
        public int AverageSecondsDriving { get; set; }
        public int TotalSecondsDriving { get; set; }
        public int ShortestRide { get; set; }
        public int LongestRide { get; set; }
        public decimal AverageDistance { get; set; }
        public decimal TotalDistance { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal MostExpensivePrice { get; set; }
        public decimal FarthestDistance { get; set; }
    }
}
