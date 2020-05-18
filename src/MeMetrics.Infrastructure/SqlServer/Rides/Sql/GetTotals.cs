namespace MeMetrics.Infrastructure.SqlServer.Rides.Sql
{
    public class GetTotals
    {
        public const string Value = @"
                  SELECT 
                      COUNT(*) as Total 
                      ,AVG(DATEDIFF(second, [RequestDate],[PickupDate])) as AverageSecondsWaiting
                      ,SUM(DATEDIFF(second, [RequestDate],[PickupDate])) as TotalSecondsWaiting
                      ,AVG(DATEDIFF(second, [PickupDate],[DropoffDate])) as AverageSecondsDriving
                      ,SUM(DATEDIFF(second, [PickupDate],[DropoffDate])) as TotalSecondsDriving
                      ,MAX(DATEDIFF(second, [PickupDate],[DropoffDate])) as LongestRide
                      ,MIN(DATEDIFF(second, [PickupDate],[DropoffDate])) as ShortestRide
                      ,AVG(Distance) as AverageDistance
                      ,SUM(Distance) as TotalDistance
                      ,AVG(Price) as AveragePrice
                      ,SUM(Price) as TotalPrice
				      ,MAX(Price) as MostExpensivePrice
				      ,Max(Distance) as FarthestDistance
                  FROM [dbo].[Ride]
                  WHERE DATEADD(d, DATEDIFF(d, 0, RequestDate), 0) >= DATEADD(d, DATEDIFF(d, 0, @startDate), 0) AND
                  DATEADD(d, DATEDIFF(d, 0, RequestDate), 0) <= DATEADD(d, DATEDIFF(d, 0, @endDate), 0);";
    }
}
