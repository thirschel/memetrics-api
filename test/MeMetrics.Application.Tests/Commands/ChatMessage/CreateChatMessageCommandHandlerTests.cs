using System.Threading;
using MeMetrics.Application.Commands.ChatMessage;
using MeMetrics.Application.Interfaces;
using MeMetrics.Application.Models;
using Moq;
using Serilog;
using Xunit;

namespace MeMetrics.Application.Tests.Commands.ChatMessage
{
    public class CreateChatMessageCommandHandlerTests
    {
        [Fact]
        public async void Request_With_No_Id_Should_Return_Invalid_Input()
        {
           // ARRANGE
           var validator = new CreateChatMessageCommandValidator();
           var chatMessageRepositoryMock = new Mock<IChatMessageRepository>();
           var loggerMock = new Mock<ILogger>();

           var handler = new CreateChatMessageCommandHandler(
               chatMessageRepositoryMock.Object,
               loggerMock.Object,
               validator
           );

           // ACT
           var response = await handler.Handle(new CreateChatMessageCommand(){ChatMessage = new Domain.Models.ChatMessage()}, new CancellationToken());

           // ASSERT
           Assert.Equal(CommandResultTypeEnum.InvalidInput, response.Type);
        }

        [Fact]
        public async void Should_ChatMessage_InsertChatMessage_In_Repository()
        {
            // ARRANGE
            var validator = new CreateChatMessageCommandValidator();
            var chatMessageRepositoryMock = new Mock<IChatMessageRepository>();
            var loggerMock = new Mock<ILogger>();

            var handler = new CreateChatMessageCommandHandler(
                chatMessageRepositoryMock.Object,
                loggerMock.Object,
                validator
            );

            chatMessageRepositoryMock.Setup(x => x.InsertChatMessage(It.IsAny<Domain.Models.ChatMessage>())).ReturnsAsync(1);

            // ACT
            var response = await handler.Handle(new CreateChatMessageCommand(){ChatMessage = new Domain.Models.ChatMessage() { ChatMessageId = "1" }}, new CancellationToken());

           // ASSERT
           Assert.Equal(CommandResultTypeEnum.Success, response.Type);
            chatMessageRepositoryMock.Verify(x => x.InsertChatMessage(It.IsAny<Domain.Models.ChatMessage>()), Times.Once);
        }

        [Fact]
        public async void Should_Return_Unprocessable_If_No_Rows_Affected()
        {
            // ARRANGE
            var validator = new CreateChatMessageCommandValidator();
            var chatMessageRepositoryMock = new Mock<IChatMessageRepository>();
            var loggerMock = new Mock<ILogger>();

            var handler = new CreateChatMessageCommandHandler(
                chatMessageRepositoryMock.Object,
                loggerMock.Object,
                validator
            );

            chatMessageRepositoryMock.Setup(x => x.InsertChatMessage(It.IsAny<Domain.Models.ChatMessage>())).ReturnsAsync(0);

            // ACT
            var response = await handler.Handle(new CreateChatMessageCommand() { ChatMessage = new Domain.Models.ChatMessage() { ChatMessageId = "1" } }, new CancellationToken());

            // ASSERT
            Assert.Equal(CommandResultTypeEnum.UnprocessableEntity, response.Type);
            chatMessageRepositoryMock.Verify(x => x.InsertChatMessage(It.IsAny<Domain.Models.ChatMessage>()), Times.Once);
        }
    }
}
