namespace MeMetrics.Infrastructure.SqlServer.Messages.Sql
{
    public class MergeMessage
    {
        public const string Value = @"
                     
                        MERGE INTO [dbo].[Message] AS TARGET
                        USING @tvp AS SOURCE 
                        ON TARGET.MessageId = SOURCE.MessageId 
                        WHEN MATCHED THEN
						UPDATE SET 
						    TARGET.PhoneNumber = SOURCE.PhoneNumber,
						    TARGET.Name = SOURCE.Name,
						    TARGET.OccurredDate = SOURCE.OccurredDate,
						    TARGET.IsIncoming = SOURCE.IsIncoming,
						    TARGET.IsMedia = SOURCE.IsMedia,
						    TARGET.Text = SOURCE.Text,
						    TARGET.TextLength = SOURCE.TextLength,
						    TARGET.ThreadId = SOURCE.ThreadId
                            WHEN NOT MATCHED THEN
                        INSERT 
                              (MessageId
                              ,PhoneNumber
                              ,Name
                              ,OccurredDate
                              ,IsIncoming
                              ,IsMedia
                              ,Text
                              ,TextLength
                              ,ThreadId)
                        VALUES (
                               SOURCE.MessageId
                              ,SOURCE.PhoneNumber
                              ,SOURCE.Name
                              ,SOURCE.OccurredDate
                              ,SOURCE.IsIncoming
                              ,SOURCE.IsMedia
                              ,SOURCE.Text
                              ,source.TextLength
                              ,source.ThreadId);";
    }
}
