namespace MeMetrics.Infrastructure.SqlServer.Calls.Sql
{
    public class GetCalls
    {
        public const string Value = @"
                    SELECT 
                        CallId
                        ,PhoneNumber
                        ,Name
                        ,OccurredDate
                        ,IsIncoming
                        ,Duration
                    FROM [dbo].[Call];";
    }
}
