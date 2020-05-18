namespace MeMetrics.Infrastructure.SqlServer.Messages.Sql
{
    public class GetTotalsByHour
    {
        public const string Value = @"
                SELECT 
                    COUNT(*) as Total
                    ,IsIncoming
                    ,DATEPART(dw,OccurredDate) as 'Day'
                    ,DATENAME(hh,OccurredDate) as 'Hour'
                FROM [dbo].[Message]
                WHERE DATEADD(d, DATEDIFF(d, 0, OccurredDate), 0) > DATEADD(d, DATEDIFF(d, 0, @startDate), 0) AND
                DATEADD(d, DATEDIFF(d, 0, OccurredDate), 0) < DATEADD(d, DATEDIFF(d, 0, @endDate), 0)
                GROUP BY DATEPART(dw,OccurredDate), DATENAME(hh,OccurredDate), IsIncoming
                ORDER BY CAST(DATENAME(hh,OccurredDate) AS INT);";
    }
}
