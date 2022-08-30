using System.Collections.Generic;
using MediatR;
using MeMetrics.Application.Models;

namespace MeMetrics.Application.Commands.Transaction
{
    public class CreateTransactionCommand : IRequest<CommandResult<bool>>
    {
        public List<Domain.Models.Transactions.Transaction> Transactions { get; set; }
    }
}
