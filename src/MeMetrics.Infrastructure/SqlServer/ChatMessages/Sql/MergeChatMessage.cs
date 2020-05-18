namespace MeMetrics.Infrastructure.SqlServer.ChatMessages.Sql
{
    public class MergeChatMessages
    {
        public const string Value = @"
                        MERGE INTO [dbo].[ChatMessage] AS TARGET
                        USING (SELECT
                             @ChatMessageId as ChatMessageId
                            ,@GroupId as GroupId
                            ,@GroupName as GroupName
                            ,@SenderName as SenderName
                            ,@SenderId as SenderId
                            ,@OccurredDate as OccurredDate
                            ,@IsIncoming as IsIncoming
                            ,@Text as Text
                            ,@IsMedia as IsMedia
                            ,@TextLength as TextLength)
                        AS SOURCE 
                        ON TARGET.ChatMessageId = SOURCE.ChatMessageId 
                        WHEN NOT MATCHED THEN
                        INSERT 
                              (ChatMessageId
                              ,GroupId
                              ,GroupName
                              ,SenderName
                              ,SenderId
                              ,OccurredDate
                              ,IsIncoming
                              ,Text
                              ,IsMedia
                              ,TextLength)
                        VALUES (
                                SOURCE.ChatMessageId
                               ,SOURCE.GroupId
                               ,SOURCE.GroupName
                               ,SOURCE.SenderName
                               ,SOURCE.SenderId
                               ,SOURCE.OccurredDate
                               ,SOURCE.IsIncoming
                               ,SOURCE.Text
                               ,SOURCE.IsMedia
                               ,SOURCE.TextLength);";
    }
}
