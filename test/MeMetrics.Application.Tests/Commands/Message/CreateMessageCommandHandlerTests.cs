using System.Threading;
using MeMetrics.Application.Commands.Message;
using MeMetrics.Application.Interfaces;
using MeMetrics.Application.Models;
using Moq;
using Serilog;
using Xunit;

namespace MeMetrics.Application.Tests.Commands.Message
{
    public class CreateMessageCommandHandlerTests
    {
        [Fact]
        public async void Request_With_No_Id_Should_Return_Invalid_Input()
        {
           // ARRANGE
           var validator = new CreateMessageCommandValidator();
           var messageRepositoryMock = new Mock<IMessageRepository>();
           var loggerMock = new Mock<ILogger>();

           var handler = new CreateMessageCommandHandler(
               messageRepositoryMock.Object,
               loggerMock.Object,
               validator
           );

           // ACT
           var response = await handler.Handle(new CreateMessageCommand(){ Message = new Domain.Models.Messages.Message() }, new CancellationToken());

           // ASSERT
           Assert.Equal(CommandResultTypeEnum.InvalidInput, response.Type);
        }

        [Fact]
        public async void Should_Message_InsertMessage_In_Repository()
        {
            // ARRANGE
            var validator = new CreateMessageCommandValidator();
            var messageRepositoryMock = new Mock<IMessageRepository>();
            var loggerMock = new Mock<ILogger>();

            var handler = new CreateMessageCommandHandler(
                messageRepositoryMock.Object,
                loggerMock.Object,
                validator
            );

            messageRepositoryMock.Setup(x => x.InsertMessage(It.IsAny<Domain.Models.Messages.Message>())).ReturnsAsync(1);

            // ACT
            var response = await handler.Handle(new CreateMessageCommand(){Message = new Domain.Models.Messages.Message() { MessageId = "1" }}, new CancellationToken());

           // ASSERT
           Assert.Equal(CommandResultTypeEnum.Success, response.Type);
            messageRepositoryMock.Verify(x => x.InsertMessage(It.IsAny<Domain.Models.Messages.Message>()), Times.Once);
        }

        [Fact]
        public async void Should_Return_Unprocessable_If_No_Rows_Affected()
        {
            // ARRANGE
            var validator = new CreateMessageCommandValidator();
            var messageRepositoryMock = new Mock<IMessageRepository>();
            var loggerMock = new Mock<ILogger>();

            var handler = new CreateMessageCommandHandler(
                messageRepositoryMock.Object,
                loggerMock.Object,
                validator
            );

            messageRepositoryMock.Setup(x => x.InsertMessage(It.IsAny<Domain.Models.Messages.Message>())).ReturnsAsync(0);

            // ACT
            var response = await handler.Handle(new CreateMessageCommand() { Message = new Domain.Models.Messages.Message() { MessageId = "1" } }, new CancellationToken());

            // ASSERT
            Assert.Equal(CommandResultTypeEnum.UnprocessableEntity, response.Type);
            messageRepositoryMock.Verify(x => x.InsertMessage(It.IsAny<Domain.Models.Messages.Message>()), Times.Once);
        }
    }
}
