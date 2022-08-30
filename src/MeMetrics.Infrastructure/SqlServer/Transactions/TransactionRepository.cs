using System;
using System.Collections.Generic;
using System.Data;
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

        public async Task<int> InsertTransactions(List<Transaction> transactions)
        {
            var tvp = new DataTable();

            tvp.Columns.Add(nameof(Transaction.TransactionId), typeof(string));
            tvp.Columns.Add(nameof(Transaction.AccountId), typeof(string));
            tvp.Columns.Add(nameof(Transaction.AccountName), typeof(string));
            tvp.Columns.Add(nameof(Transaction.MerchantId), typeof(string));
            tvp.Columns.Add(nameof(Transaction.Amount), typeof(decimal));
            tvp.Columns.Add(nameof(Transaction.Description), typeof(string));
            tvp.Columns.Add(nameof(Transaction.IsCashIn), typeof(bool));
            tvp.Columns.Add(nameof(Transaction.IsCashOut), typeof(bool));
            tvp.Columns.Add(nameof(Transaction.CategoryId), typeof(int));
            tvp.Columns.Add(nameof(Transaction.Labels), typeof(string));
            tvp.Columns.Add(nameof(Transaction.OccurredDate), typeof(DateTimeOffset));

            foreach (var transaction in transactions)
            {
                var row = tvp.NewRow();
                row[nameof(transaction.TransactionId)] = transaction.TransactionId;
                row[nameof(transaction.AccountId)] = transaction.AccountId;
                row[nameof(transaction.AccountName)] = transaction.AccountName;
                row[nameof(transaction.MerchantId)] = transaction.MerchantId;
                row[nameof(transaction.Amount)] = transaction.Amount;
                row[nameof(transaction.Description)] = transaction.Description;
                row[nameof(transaction.IsCashIn)] = transaction.IsCashIn;
                row[nameof(transaction.IsCashOut)] = transaction.IsCashOut;
                row[nameof(transaction.CategoryId)] = transaction.CategoryId;
                row[nameof(transaction.Labels)] = transaction.Labels;
                row[nameof(transaction.OccurredDate)] = transaction.OccurredDate;

                tvp.Rows.Add(row);
            }
            using (var connection = SqlConnectionBuilder.Build(_configuration.Value.DB_CONNECTION_STRING))
            {
                await connection.ExecuteAsync(MergeTransaction.Value, new {tvp = tvp.AsTableValuedParameter("dbo.TransactionType") });
            }

            return 1;
        }

    }
}
