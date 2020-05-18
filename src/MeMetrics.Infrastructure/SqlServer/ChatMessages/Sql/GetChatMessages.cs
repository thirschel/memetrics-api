namespace MeMetrics.Infrastructure.SqlServer.ChatMessages.Sql
{
    public class GetChatMessages
    {
        public const string Value = @"
                    SELECT 
                       ChatMessageId
                      ,GroupId
                      ,GroupName
                      ,SenderName
                      ,SenderId
                      ,OccurredDate
                      ,IsIncoming
                      ,Text
                      ,IsMedia
                      ,TextLength
                    FROM [dbo].[ChatMessage];";
    }
}
