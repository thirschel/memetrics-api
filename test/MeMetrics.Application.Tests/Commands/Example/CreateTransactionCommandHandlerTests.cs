using System.Threading;
using MeMetrics.Application.Models;
using MeMetrics.Application.Interfaces;
using Moq;
using Serilog;
using Xunit;
using MeMetrics.Application.Commands.Transaction;
using MeMetrics.Domain.Models.Transactions;

namespace MeMetrics.Application.Tests.Commands.Example
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
           var response = await handler.Handle(new CreateTransactionCommand() { Transaction = new Transaction() }, new CancellationToken());

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

            transactionRepositoryMock.Setup(x => x.InsertTransaction(It.IsAny<Transaction>())).ReturnsAsync(1);

            // ACT
            var response = await handler.Handle(new CreateTransactionCommand(){Transaction = new Transaction() { TransactionId = "1" }}, new CancellationToken());

           // ASSERT
           Assert.Equal(CommandResultTypeEnum.Success, response.Type);
            transactionRepositoryMock.Verify(x => x.InsertTransaction(It.IsAny<Transaction>()), Times.Once);
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

            transactionRepositoryMock.Setup(x => x.InsertTransaction(It.IsAny<Transaction>())).ReturnsAsync(0);

            // ACT
            var response = await handler.Handle(new CreateTransactionCommand() { Transaction = new Transaction() { TransactionId = "1" } }, new CancellationToken());

            // ASSERT
            Assert.Equal(CommandResultTypeEnum.UnprocessableEntity, response.Type);
            transactionRepositoryMock.Verify(x => x.InsertTransaction(It.IsAny<Transaction>()), Times.Once);
        }
    }
}
