namespace MeMetrics.Infrastructure.SqlServer.Transactions.Sql
{
    public class GetTransactions
    {
        public const string Value = @"
                    SELECT 
                       TransactionId
                      ,AccountId
                      ,AccountName
                      ,MerchantId
                      ,Amount
                      ,Description
                      ,IsCashIn
                      ,IsCashOut
                      ,CategoryId
                      ,Labels
                      ,OccurredDate
                    FROM [dbo].[Transaction];";
    }
}
