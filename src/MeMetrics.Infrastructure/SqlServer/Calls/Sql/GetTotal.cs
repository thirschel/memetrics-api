namespace MeMetrics.Infrastructure.SqlServer.Calls.Sql
{
    public class GetTotal
    {
        public const string Value = @"
                  SELECT 
                    COUNT(*) as TotalCalls
                  FROM [dbo].[Call]
                  WHERE DATEADD(d, DATEDIFF(d, 0, OccurredDate), 0) > DATEADD(d, DATEDIFF(d, 0, @startDate), 0) AND
                  DATEADD(d, DATEDIFF(d, 0, OccurredDate), 0) < DATEADD(d, DATEDIFF(d, 0, @endDate), 0);";
    }
}
