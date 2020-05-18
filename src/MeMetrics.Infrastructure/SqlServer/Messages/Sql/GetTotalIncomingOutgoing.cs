namespace MeMetrics.Infrastructure.SqlServer.Messages.Sql
{
    public class GetTotalIncomingOutgoing
    {
        public const string Value = @"
                 SELECT 
                        COUNT(*) as Total
                        ,IsIncoming
                    FROM [dbo].[Message]
                    WHERE DATEADD(d, DATEDIFF(d, 0, OccurredDate), 0) > DATEADD(d, DATEDIFF(d, 0, @startDate), 0) AND
                    DATEADD(d, DATEDIFF(d, 0, OccurredDate), 0) < DATEADD(d, DATEDIFF(d, 0, @endDate), 0)
                    GROUP BY IsIncoming
                    ORDER BY Total Desc;";
    }
}
