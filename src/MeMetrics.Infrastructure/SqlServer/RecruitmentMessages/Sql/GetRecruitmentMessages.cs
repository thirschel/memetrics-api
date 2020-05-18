namespace MeMetrics.Infrastructure.SqlServer.RecruitmentMessages.Sql
{
    public class GetRecruitmentMessages
    {
        public const string Value = @"
                    SELECT 
                       RecruitmentMessageId
                      ,RecruiterId
                      ,MessageSource
                      ,RecruiterName
                      ,RecruiterCompany
                      ,Subject
                      ,Body
                      ,IsIncoming
                      ,OccurredDate
                    FROM [dbo].[RecruitmentMessage];";
    }
}
