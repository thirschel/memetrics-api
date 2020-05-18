namespace MeMetrics.Infrastructure.SqlServer.Calls.Sql
{
    public class GetTotalsByPeriod
    {
        public const string Value = @"
                  SELECT
                        CASE WHEN DATEADD(d, DATEDIFF(d, 0, OccurredDate), 0) < DATEADD(d, DATEDIFF(d, 0, @previousPeriodStartDate), 0) THEN 1 ELSE 0 END as IsPreviousPeriod
                        ,DATEADD(d, DATEDIFF(d, 0, OccurredDate), 0) as OccurredDate
                        ,COUNT(*) as Count
                    FROM [dbo].[Call]
                    WHERE DATEADD(d, DATEDIFF(d, 0, OccurredDate), 0) > DATEADD(d, DATEDIFF(d, 0, @previousPeriodStartDate), 0) AND 
                    DATEADD(d, DATEDIFF(d, 0, OccurredDate), 0) <= DATEADD(d, DATEDIFF(d, 0, @endDate), 0)
                    GROUP BY DATEADD(d, DATEDIFF(d, 0, OccurredDate), 0), DATEPART(YYYY, OccurredDate)
                    ORDER BY OccurredDate;";
    }
}
