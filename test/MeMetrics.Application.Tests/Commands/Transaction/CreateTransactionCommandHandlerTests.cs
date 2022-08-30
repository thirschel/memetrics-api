using System.Collections.Generic;
using System.Threading;
using MeMetrics.Application.Commands.Transaction;
using MeMetrics.Application.Interfaces;
using MeMetrics.Application.Models;
using Moq;
using Serilog;
using Xunit;

namespace MeMetrics.Application.Tests.Commands.Transaction
{
    public class CreateTransactionCommandHandlerTests
    {
        [Fact]
        public async void Request_With_No_Id_Should_Return_Invalid_Input()
        {
           // ARRANGE
           var validator = new CreateTransactionCommandValidator();
           var transactionRepositoryMock = new Mock<ITransactionRepository>();
           var loggerMock = new Mock<ILogger>();

           var handler = new CreateTransactionCommandHandler(
               transactionRepositoryMock.Object,
               loggerMock.Object,
               validator
           );

           // ACT
           var response = await handler.Handle(new CreateTransactionCommand() { Transactions = new List<Domain.Models.Transactions.Transaction>() { new Domain.Models.Transactions.Transaction() }}, new CancellationToken());

           // ASSERT
           Assert.Equal(CommandResultTypeEnum.InvalidInput, response.Type);
        }

        [Fact]
        public async void Should_Transaction_InsertTransaction_In_Repository()
        {
            // ARRANGE
            var validator = new CreateTransactionCommandValidator();
            var transactionRepositoryMock = new Mock<ITransactionRepository>();
            var loggerMock = new Mock<ILogger>();

            var handler = new CreateTransactionCommandHandler(
                transactionRepositoryMock.Object,
                loggerMock.Object,
                validator
            );

            transactionRepositoryMock.Setup(x => x.InsertTransactions(It.IsAny<List<Domain.Models.Transactions.Transaction>>())).ReturnsAsync(1);

            // ACT
            var response = await handler.Handle(new CreateTransactionCommand(){Transactions = new List<Domain.Models.Transactions.Transaction>() { new Domain.Models.Transactions.Transaction() { TransactionId = "1" }}}, new CancellationToken());

           // ASSERT
           Assert.Equal(CommandResultTypeEnum.Success, response.Type);
            transactionRepositoryMock.Verify(x => x.InsertTransactions(It.IsAny<List<Domain.Models.Transactions.Transaction>>()), Times.Once);
        }

        [Fact]
        public async void Should_Return_Unprocessable_If_No_Rows_Affected()
        {
            // ARRANGE
            var validator = new CreateTransactionCommandValidator();
            var transactionRepositoryMock = new Mock<ITransactionRepository>();
            var loggerMock = new Mock<ILogger>();

            var handler = new CreateTransactionCommandHandler(
                transactionRepositoryMock.Object,
                loggerMock.Object,
                validator
            );

            transactionRepositoryMock.Setup(x => x.InsertTransactions(It.IsAny<List<Domain.Models.Transactions.Transaction>>())).ReturnsAsync(0);

            // ACT
            var response = await handler.Handle(new CreateTransactionCommand() { Transactions = new List<Domain.Models.Transactions.Transaction>() { new Domain.Models.Transactions.Transaction() { TransactionId = "1" } } }, new CancellationToken());

            // ASSERT
            Assert.Equal(CommandResultTypeEnum.UnprocessableEntity, response.Type);
            transactionRepositoryMock.Verify(x => x.InsertTransactions(It.IsAny<List<Domain.Models.Transactions.Transaction>>()), Times.Once);
        }
    }
}
