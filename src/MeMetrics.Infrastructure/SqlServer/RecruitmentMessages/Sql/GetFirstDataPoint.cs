namespace MeMetrics.Infrastructure.SqlServer.RecruitmentMessages.Sql
{
    public class GetFirstDataPoint
    {
        public const string Value = @"
                    SELECT TOP 1 DATEADD(d, DATEDIFF(d, 0, OccurredDate), 0) FROM [dbo].[RecruitmentMessage] ORDER BY OccurredDate ASC;";
    }
}

