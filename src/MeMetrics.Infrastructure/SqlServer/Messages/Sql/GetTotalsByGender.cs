namespace MeMetrics.Infrastructure.SqlServer.Messages.Sql
{
    public class GetTotalsByGender
    {
        public const string Value = @"
                  SELECT 
                         COUNT(*) as Total
                        ,Gender
                        ,IsIncoming
                        ,AVG(TextLength) as AverageTextLength
                    FROM MessageWithName
                    WHERE DATEADD(d, DATEDIFF(d, 0, OccurredDate), 0) > DATEADD(d, DATEDIFF(d, 0, @startDate), 0) AND
                    DATEADD(d, DATEDIFF(d, 0, OccurredDate), 0) < DATEADD(d, DATEDIFF(d, 0, @endDate), 0) AND Gender IS NOT NULL
                    GROUP BY Gender, IsIncoming
                    ORDER BY Total Desc;";
    }
}
