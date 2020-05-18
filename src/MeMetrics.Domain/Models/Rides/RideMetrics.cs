using System.Collections.Generic;

namespace MeMetrics.Domain.Models.Rides
{
    public class RideMetrics
    {
        public int TotalRides { get; set; }
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
        public IEnumerable<RideByDayOfWeek> RidesByDayOfWeek { get; set; }
        public IEnumerable<RideByHour> RidesByHour { get; set; }
        public IEnumerable<ByPeriod> RidePerformance { get; set; }
        public IEnumerable<ByWeek> WeekOverWeek { get; set; }
        public string CurrentPeriodLabel { get; set; }
        public string PriorPeriodLabel { get; set; }
    }
}