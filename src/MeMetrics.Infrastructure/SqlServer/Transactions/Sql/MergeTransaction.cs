namespace MeMetrics.Infrastructure.SqlServer.Transactions.Sql
{
    public class MergeTransaction
    {
        public const string Value = @"
                        MERGE INTO [dbo].[Transaction] AS TARGET
                        USING @tvp AS SOURCE 
                        ON TARGET.TransactionId = SOURCE.TransactionId 
                        WHEN MATCHED THEN
						UPDATE SET 
						    TARGET.AccountId = SOURCE.AccountId,
						    TARGET.AccountName = SOURCE.AccountName,
						    TARGET.MerchantId = SOURCE.MerchantId,
						    TARGET.Amount = SOURCE.Amount,
						    TARGET.Description = SOURCE.Description,
						    TARGET.IsCashIn = SOURCE.IsCashIn,
						    TARGET.IsCashOut = SOURCE.IsCashOut,
						    TARGET.CategoryId = SOURCE.CategoryId,
						    TARGET.Labels = SOURCE.Labels,
						    TARGET.OccurredDate = SOURCE.OccurredDate
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
