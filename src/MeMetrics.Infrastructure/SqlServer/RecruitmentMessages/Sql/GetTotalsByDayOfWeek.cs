namespace MeMetrics.Infrastructure.SqlServer.RecruitmentMessages.Sql
{
    public class GetTotalsByDayOfWeek
    {
        public const string Value = @"
                  SELECT 
                    COUNT(*) as Total
                    ,IsIncoming
                    ,DATEPART(dw, OccurredDate) as 'DayOfWeek'
                  FROM [dbo].[RecruitmentMessage]
                  WHERE DATEADD(d, DATEDIFF(d, 0, OccurredDate), 0) > DATEADD(d, DATEDIFF(d, 0, @startDate), 0) AND
                  DATEADD(d, DATEDIFF(d, 0, OccurredDate), 0) < DATEADD(d, DATEDIFF(d, 0, @endDate), 0)
                  GROUP BY DATEPART(dw, OccurredDate), IsIncoming
                  ORDER BY DATEPART(dw, OccurredDate) ASC;";
    }
}
