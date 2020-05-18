namespace MeMetrics.Infrastructure.SqlServer.Calls.Sql
{
    public class GetTotalsByWeek
    {
        public const string Value = @"
                    SELECT 
                        DATEADD(day, DATEDIFF(day, 0, c.Date) /7*7, 0) as WeekOf
                        ,COUNT(data.OccurredDate) as Total
					FROM [Auxiliary].[Calendar] c
                    LEFT JOIN [dbo].[Call] data on c.Date = DATEADD(d, DATEDIFF(d, 0, data.OccurredDate), 0)
                    WHERE
					DATEADD(d, DATEDIFF(d, 0, c.Date), 0) >= DATEADD(d, DATEDIFF(d, 0, @startDate), 0) AND
					DATEADD(d, DATEDIFF(d, 0, c.Date), 0) <= DATEADD(d, DATEDIFF(d, 0, @endDate), 0)   AND
                    DATEADD(d, DATEDIFF(d, 0, c.Date), 0) <= DATEADD(d, DATEDIFF(d, 0, GETDATE()), 0)
                    GROUP BY DATEADD(day, DATEDIFF(day, 0, c.Date) /7*7, 0)
                    ORDER BY DATEADD(day, DATEDIFF(day, 0, c.Date) /7*7, 0) ASC";
    }
}
