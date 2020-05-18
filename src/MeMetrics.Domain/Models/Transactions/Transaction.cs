using System;

namespace MeMetrics.Domain.Models.Transactions
{
    public class Transaction
    {
        public string TransactionId { get; set; }
        public string AccountId { get; set; }
        public string AccountName { get; set; }
        public string MerchantId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public bool IsCashIn { get; set; }
        public bool IsCashOut { get; set; }
        public int CategoryId { get; set; }
        public string Labels { get; set; }
        public DateTime OccurredDate { get; set; }
    }
}