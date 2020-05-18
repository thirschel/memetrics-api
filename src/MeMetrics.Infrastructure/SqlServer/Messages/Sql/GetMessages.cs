namespace MeMetrics.Infrastructure.SqlServer.Messages.Sql
{
    public class GetMessages
    {
        public const string Value = @"
                    SELECT 
                       MessageId
                      ,PhoneNumber
                      ,Name
                      ,OccurredDate
                      ,IsIncoming
                      ,IsMedia
                      ,Text
                      ,TextLength
                      ,ThreadId
                    FROM [dbo].[Message];";
    }
}
