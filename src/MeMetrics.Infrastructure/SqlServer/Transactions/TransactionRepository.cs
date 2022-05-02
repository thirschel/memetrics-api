using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using MeMetrics.Application.Interfaces;
using MeMetrics.Application.Models;
using MeMetrics.Domain.Models.Transactions;
using MeMetrics.Infrastructure.Helpers;
using MeMetrics.Infrastructure.SqlServer.Transactions.Sql;
using Microsoft.Extensions.Options;

namespace MeMetrics.Infrastructure.SqlServer.Transactions
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IOptions<EnvironmentConfiguration> _configuration;
        private readonly IMapper _mapper;

        public TransactionRepository(
            IOptions<EnvironmentConfiguration> configuration,
            IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<List<Transaction>> GetTransactions()
        {
            using (var connection = SqlConnectionBuilder.Build(_configuration.Value.DB_CONNECTION_STRING))
            {
                var calls = await connection.QueryAsync<Transaction>(Sql.GetTransactions.Value);
                return calls.ToList();
            }
        }

        public async Task<int> InsertTransaction(Transaction transaction)
        {
            using (var connection = SqlConnectionBuilder.Build(_configuration.Value.DB_CONNECTION_STRING))
            {
                await connection.ExecuteAsync(MergeTransaction.Value, new
                {
                    transaction.TransactionId,
                    transaction.AccountId,
                    transaction.AccountName,
                    transaction.MerchantId,
                    transaction.Amount,
                    transaction.Description,
                    transaction.IsCashIn,
                    transaction.IsCashOut,
                    transaction.CategoryId,
                    transaction.Labels,
                    transaction.OccurredDate,
                });
            }

            return 1;
        }

    }
}
