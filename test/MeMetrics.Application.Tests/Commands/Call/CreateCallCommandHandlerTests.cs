using System.Threading;
using MeMetrics.Application.Commands.Call;
using MeMetrics.Application.Interfaces;
using MeMetrics.Application.Models;
using Moq;
using Serilog;
using Xunit;

namespace MeMetrics.Application.Tests.Commands.Call
{
    public class CreateCallCommandHandlerTests
    {
        [Fact]
        public async void Request_With_No_Id_Should_Return_Invalid_Input()
        {
           // ARRANGE
           var validator = new CreateCallCommandValidator();
           var callRepositoryMock = new Mock<ICallRepository>();
           var loggerMock = new Mock<ILogger>();

           var handler = new CreateCallCommandHandler(
               callRepositoryMock.Object,
               loggerMock.Object,
               validator
           );

           // ACT
           var response = await handler.Handle(new CreateCallCommand(), new CancellationToken());

           // ASSERT
           Assert.Equal(CommandResultTypeEnum.InvalidInput, response.Type);
        }

        [Fact]
        public async void Should_Call_InsertCall_In_Repository()
        {
            // ARRANGE
            var validator = new CreateCallCommandValidator();
            var callRepositoryMock = new Mock<ICallRepository>();
            var loggerMock = new Mock<ILogger>();

            var handler = new CreateCallCommandHandler(
                callRepositoryMock.Object,
                loggerMock.Object,
                validator
            );

            callRepositoryMock.Setup(x => x.InsertCall(It.IsAny<Domain.Models.Calls.Call>())).ReturnsAsync(1);

            // ACT
            var response = await handler.Handle(new CreateCallCommand(){Call = new Domain.Models.Calls.Call() { CallId = "1" }}, new CancellationToken());

           // ASSERT
           Assert.Equal(CommandResultTypeEnum.Success, response.Type);
            callRepositoryMock.Verify(x => x.InsertCall(It.IsAny<Domain.Models.Calls.Call>()), Times.Once);
        }

        [Fact]
        public async void Should_Return_Unprocessable_If_No_Rows_Affected()
        {
            // ARRANGE
            var validator = new CreateCallCommandValidator();
            var callRepositoryMock = new Mock<ICallRepository>();
            var loggerMock = new Mock<ILogger>();

            var handler = new CreateCallCommandHandler(
                callRepositoryMock.Object,
                loggerMock.Object,
                validator
            );

            callRepositoryMock.Setup(x => x.InsertCall(It.IsAny<Domain.Models.Calls.Call>())).ReturnsAsync(0);

            // ACT
            var response = await handler.Handle(new CreateCallCommand() { Call = new Domain.Models.Calls.Call() { CallId = "1" } }, new CancellationToken());

            // ASSERT
            Assert.Equal(CommandResultTypeEnum.UnprocessableEntity, response.Type);
            callRepositoryMock.Verify(x => x.InsertCall(It.IsAny<Domain.Models.Calls.Call>()), Times.Once);
        }
    }
}
