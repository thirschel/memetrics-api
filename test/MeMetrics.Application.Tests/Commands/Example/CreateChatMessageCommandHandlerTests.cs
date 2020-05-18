using System.Threading;
using MeMetrics.Application.Models;
using MeMetrics.Application.Interfaces;
using Moq;
using Serilog;
using Xunit;
using MeMetrics.Application.Commands.Call;
using MeMetrics.Domain.Models.Calls;
using MeMetrics.Application.Commands.ChatMessage;
using MeMetrics.Domain.Models;

namespace MeMetrics.Application.Tests.Commands.Example
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
           var response = await handler.Handle(new CreateChatMessageCommand(){ChatMessage = new ChatMessage()}, new CancellationToken());

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

            chatMessageRepositoryMock.Setup(x => x.InsertChatMessage(It.IsAny<ChatMessage>())).ReturnsAsync(1);

            // ACT
            var response = await handler.Handle(new CreateChatMessageCommand(){ChatMessage = new ChatMessage() { ChatMessageId = "1" }}, new CancellationToken());

           // ASSERT
           Assert.Equal(CommandResultTypeEnum.Success, response.Type);
            chatMessageRepositoryMock.Verify(x => x.InsertChatMessage(It.IsAny<ChatMessage>()), Times.Once);
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

            chatMessageRepositoryMock.Setup(x => x.InsertChatMessage(It.IsAny<ChatMessage>())).ReturnsAsync(0);

            // ACT
            var response = await handler.Handle(new CreateChatMessageCommand() { ChatMessage = new ChatMessage() { ChatMessageId = "1" } }, new CancellationToken());

            // ASSERT
            Assert.Equal(CommandResultTypeEnum.UnprocessableEntity, response.Type);
            chatMessageRepositoryMock.Verify(x => x.InsertChatMessage(It.IsAny<ChatMessage>()), Times.Once);
        }
    }
}
