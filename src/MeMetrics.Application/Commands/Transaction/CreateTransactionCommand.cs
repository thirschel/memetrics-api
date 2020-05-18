using MediatR;
using MeMetrics.Application.Models;

namespace MeMetrics.Application.Commands.Transaction
{
    public class CreateTransactionCommand : IRequest<CommandResult<bool>>
    {
        public Domain.Models.Transactions.Transaction Transaction { get; set; }
    }
}
