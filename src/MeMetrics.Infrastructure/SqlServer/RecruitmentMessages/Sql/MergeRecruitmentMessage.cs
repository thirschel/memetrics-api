namespace MeMetrics.Infrastructure.SqlServer.RecruitmentMessages.Sql
{
    public class MergeRecruitmentMessage
    {
        public const string Value = @"
                        MERGE INTO [dbo].[RecruitmentMessage] AS TARGET
                        USING @tvp AS SOURCE 
                        ON TARGET.RecruitmentMessageId = SOURCE.RecruitmentMessageId 
                        WHEN MATCHED THEN
						UPDATE SET 
                            TARGET.RecruiterId = SOURCE.RecruiterId,
                            TARGET.MessageSource = SOURCE.MessageSource,
                            TARGET.RecruiterName = SOURCE.RecruiterName,
                            TARGET.RecruiterCompany = SOURCE.RecruiterCompany,
                            TARGET.Subject = SOURCE.Subject,
                            TARGET.Body = SOURCE.Body,
                            TARGET.IsIncoming = SOURCE.IsIncoming,
                            TARGET.OccurredDate = SOURCE.OccurredDate
                        WHEN NOT MATCHED THEN
                        INSERT 
                              (RecruitmentMessageId
                              ,RecruiterId
                              ,MessageSource
                              ,RecruiterName
                              ,RecruiterCompany
                              ,Subject
                              ,Body
                              ,IsIncoming
                              ,OccurredDate)
                        VALUES (
                                SOURCE.RecruitmentMessageId
                               ,SOURCE.RecruiterId
                               ,SOURCE.MessageSource
                               ,SOURCE.RecruiterName
                               ,SOURCE.RecruiterCompany
                               ,SOURCE.Subject
                               ,SOURCE.Body
                               ,SOURCE.IsIncoming
                               ,SOURCE.OccurredDate);";
    }
}
