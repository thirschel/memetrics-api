using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MeMetrics.Application.Models;
using Moq;
using Serilog;
using Xunit;
using MeMetrics.Application.Commands.Call;
using MeMetrics.Api.Controllers;
using MeMetrics.Application.Commands.Transaction;
using MeMetrics.Domain.Models.Transactions;

namespace MeMetrics.Api.Tests
{
    public class TransactionControllerTests
    {
        [Fact]
        public async void SaveTransaction_Should_Return_Ok_Result()
        {
           var mediatorMock = new Mock<IMediator>();
           var loggerMock = new Mock<ILogger>();

           mediatorMock
               .Setup(x => x.Send(It.IsAny<CreateTransactionCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new CommandResult<bool>(){ Type = CommandResultTypeEnum.Success });
           var controller = new TransactionController(
               loggerMock.Object,
               mediatorMock.Object
           );

           var response = await controller.SaveTransaction(new Transaction());

           Assert.IsType<OkObjectResult>(response.Result);
           mediatorMock.Verify(x => x.Send(It.IsAny<CreateTransactionCommand>(), It.IsAny<CancellationToken>()), Times.Once());

        }

        [Fact]
        public async void SaveChatMessage_Should_Return_Bad_Result_If_Invalid_Input()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger>();

            mediatorMock
                .Setup(x => x.Send(It.IsAny<CreateTransactionCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CommandResult<bool>() { Type = CommandResultTypeEnum.InvalidInput });
            var controller = new TransactionController(
                loggerMock.Object,
                mediatorMock.Object
            );

            var response = await controller.SaveTransaction(new Transaction());

            Assert.IsType<BadRequestResult>(response.Result);
        }
    }
}
