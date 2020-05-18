namespace MeMetrics.Infrastructure.SqlServer.Transactions.Sql
{
    public class MergeTransaction
    {
        public const string Value = @"
                        MERGE INTO [dbo].[Transaction] AS TARGET
                        USING (SELECT
                             @TransactionId as TransactionId
                            ,@AccountId as AccountId
                            ,@AccountName as AccountName
                            ,@MerchantId as MerchantId
                            ,@Amount as Amount
                            ,@Description as Description
                            ,@IsCashIn as IsCashIn
                            ,@IsCashOut as IsCashOut
                            ,@CategoryId as CategoryId
                            ,@Labels as Labels
                            ,@OccurredDate as OccurredDate)
                        AS SOURCE 
                        ON TARGET.TransactionId = SOURCE.TransactionId 
                        WHEN NOT MATCHED THEN
                        INSERT 
                              (TransactionId
                              ,AccountId
                              ,AccountName
                              ,MerchantId
                              ,Amount
                              ,Description
                              ,IsCashIn
                              ,IsCashOut
                              ,CategoryId
                              ,Labels
                              ,OccurredDate)
                        VALUES (
                                SOURCE.TransactionId
                               ,SOURCE.AccountId
                               ,SOURCE.AccountName
                               ,SOURCE.MerchantId
                               ,SOURCE.Amount
                               ,SOURCE.Description
                               ,SOURCE.IsCashIn
                               ,SOURCE.IsCashOut
                               ,SOURCE.CategoryId
                               ,SOURCE.Labels
                               ,SOURCE.OccurredDate);";
    }
}
