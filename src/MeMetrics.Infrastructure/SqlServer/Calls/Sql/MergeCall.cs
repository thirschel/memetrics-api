namespace MeMetrics.Infrastructure.SqlServer.Calls.Sql
{
    public class MergeCall
    {
        public const string Value = @"
                        MERGE INTO [dbo].[Call] AS TARGET
                        USING (SELECT
                             @CallId as CallId
                            ,@PhoneNumber as PhoneNumber
                            ,@Name as Name
                            ,@OccurredDate as OccurredDate
                            ,@IsIncoming as IsIncoming
                            ,@Duration as Duration)
                        AS SOURCE 
                        ON TARGET.CallId = SOURCE.CallId 
                        WHEN NOT MATCHED THEN
                        INSERT 
                              (CallId
                              ,PhoneNumber
                              ,Name
                              ,OccurredDate
                              ,IsIncoming
                              ,Duration)
                        VALUES (
                                SOURCE.CallId
                               ,SOURCE.PhoneNumber
                               ,SOURCE.Name
                               ,SOURCE.OccurredDate
                               ,SOURCE.IsIncoming
                               ,SOURCE.Duration);";
    }
}
