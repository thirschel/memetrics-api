namespace MeMetrics.Infrastructure.SqlServer.Messages.Sql
{
    public class GetFirstDataPoint
    {
        public const string Value = @"
                    SELECT TOP 1 DATEADD(d, DATEDIFF(d, 0, OccurredDate), 0) FROM [dbo].[Message] ORDER BY OccurredDate ASC;";
    }
}

