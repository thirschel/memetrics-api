namespace MeMetrics.Infrastructure.SqlServer.Rides.Sql
{
    public class GetTotalsByDayOfWeek
    {
        public const string Value = @"
                  SELECT 
                    COUNT(*) as Total, 
                    DATEPART(dw, RequestDate) as 'DayOfWeek'
                  FROM [dbo].[Ride]
                  WHERE DATEADD(d, DATEDIFF(d, 0, RequestDate), 0) >= DATEADD(d, DATEDIFF(d, 0, @startDate), 0) AND
                  DATEADD(d, DATEDIFF(d, 0, RequestDate), 0) <= DATEADD(d, DATEDIFF(d, 0, @endDate), 0)
                  GROUP BY DATEPART(dw, RequestDate)
                  ORDER BY DATEPART(dw, RequestDate) ASC;";
    }
}
