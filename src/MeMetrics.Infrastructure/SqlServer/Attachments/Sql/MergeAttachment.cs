namespace MeMetrics.Infrastructure.SqlServer.Attachments.Sql
{
    public class MergeAttachment
    {
        public const string Value = @"MERGE INTO Attachment AS TARGET
                        USING (SELECT
                             @AttachmentId AS AttachmentId
                            ,@MessageId AS MessageId
                            ,@Base64Data AS Base64Data
                            ,@FileName AS FileName)
                        AS SOURCE 
                        ON TARGET.AttachmentId = SOURCE.AttachmentId 
                        WHEN MATCHED THEN
						UPDATE SET 
						    TARGET.MessageId = SOURCE.MessageId,
						    TARGET.Base64Data = SOURCE.Base64Data,
						    TARGET.FileName = SOURCE.FileName
                            WHEN NOT MATCHED THEN
                        INSERT 
                              (AttachmentId
                              ,MessageId
                              ,Base64Data
                              ,FileName)
                        VALUES (
                               SOURCE.AttachmentId
                              ,SOURCE.MessageId
                              ,SOURCE.Base64Data
                              ,SOURCE.FileName);";
    }
}
