namespace MeMetrics.Infrastructure.SqlServer.RecruitmentMessages.Sql
{
    public class GetTotal
    {
        public const string Value = @"
                  SELECT 
                      COUNT(*) as Total
                      ,COUNT(DISTINCT RecruiterId) as UniqueContacts
                  FROM RecruitmentMessage
                  WHERE DATEADD(d, DATEDIFF(d, 0, OccurredDate), 0) > DATEADD(d, DATEDIFF(d, 0, @startDate), 0) AND
                  DATEADD(d, DATEDIFF(d, 0, OccurredDate), 0) < DATEADD(d, DATEDIFF(d, 0, @endDate), 0);";
    }
}
