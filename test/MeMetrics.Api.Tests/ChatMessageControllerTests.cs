using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MeMetrics.Application.Models;
using Moq;
using Serilog;
using Xunit;
using MeMetrics.Application.Commands.Call;
using MeMetrics.Api.Controllers;
using MeMetrics.Application.Commands.ChatMessage;
using MeMetrics.Domain.Models;

namespace MeMetrics.Api.Tests
{
    public class ChatMessageControllerTests
    {
        [Fact]
        public async void SaveChatMessage_Should_Return_Ok_Result()
        {
           var mediatorMock = new Mock<IMediator>();
           var loggerMock = new Mock<ILogger>();

           mediatorMock
               .Setup(x => x.Send(It.IsAny<CreateChatMessageCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new CommandResult<bool>(){ Type = CommandResultTypeEnum.Success });
           var controller = new ChatMessageController(
               loggerMock.Object,
               mediatorMock.Object
           );

           var response = await controller.SaveChatMessage(new ChatMessage());

           Assert.IsType<OkObjectResult>(response.Result);
           mediatorMock.Verify(x => x.Send(It.IsAny<CreateChatMessageCommand>(), It.IsAny<CancellationToken>()), Times.Once());

        }

        [Fact]
        public async void SaveChatMessage_Should_Return_Bad_Result_If_Invalid_Input()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger>();

            mediatorMock
                .Setup(x => x.Send(It.IsAny<CreateChatMessageCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CommandResult<bool>() { Type = CommandResultTypeEnum.InvalidInput });
            var controller = new ChatMessageController(
                loggerMock.Object,
                mediatorMock.Object
            );

            var response = await controller.SaveChatMessage(new ChatMessage());

            Assert.IsType<BadRequestResult>(response.Result);
        }
    }
}
