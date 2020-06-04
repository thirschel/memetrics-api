namespace MeMetrics.Infrastructure.SqlServer.Calls.Sql
{
    public class GetTotalKnownMaleDuration
    {
        public const string Value = @"
                  SELECT
                    ISNULL(SUM(Duration), 0) as TotalDurationSeconds
                  FROM CallWithName
                  WHERE DATEADD(d, DATEDIFF(d, 0, OccurredDate), 0) > DATEADD(d, DATEDIFF(d, 0, @startDate), 0) AND
                  DATEADD(d, DATEDIFF(d, 0, OccurredDate), 0) < DATEADD(d, DATEDIFF(d, 0, @endDate), 0) AND
                  Gender = 'M';";
    }
}
