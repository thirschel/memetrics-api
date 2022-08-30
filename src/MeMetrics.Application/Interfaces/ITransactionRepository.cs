using System.Collections.Generic;
using System.Threading.Tasks;
using MeMetrics.Domain.Models.Transactions;

namespace MeMetrics.Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetTransactions();
        Task<int> InsertTransactions(List<Transaction> transactions);
    }
}
