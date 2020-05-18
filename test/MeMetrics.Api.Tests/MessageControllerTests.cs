using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MeMetrics.Application.Models;
using Moq;
using Serilog;
using Xunit;
using MeMetrics.Api.Controllers;
using MeMetrics.Application.Commands.Message;
using MeMetrics.Application.Models.Enums;
using MeMetrics.Domain.Models.Messages;
using MeMetrics.Application.Queries.Messages;

namespace MeMetrics.Api.Tests
{
    public class MessageControllerTests
    {
        [Fact]
        public async void SaveMessage_Should_Return_Ok_Result()
        {
           var mediatorMock = new Mock<IMediator>();
           var loggerMock = new Mock<ILogger>();

           mediatorMock
               .Setup(x => x.Send(It.IsAny<CreateMessageCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new CommandResult<bool>(){ Type = CommandResultTypeEnum.Success });
           var controller = new MessageController(
               loggerMock.Object,
               mediatorMock.Object
           );

           var response = await controller.SaveMessage(new Message());

           Assert.IsType<OkObjectResult>(response.Result);
           mediatorMock.Verify(x => x.Send(It.IsAny<CreateMessageCommand>(), It.IsAny<CancellationToken>()), Times.Once());

        }

        [Fact]
        public async void SaveMessage_Should_Return_Bad_Result_If_Invalid_Input()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger>();

            mediatorMock
                .Setup(x => x.Send(It.IsAny<CreateMessageCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CommandResult<bool>() { Type = CommandResultTypeEnum.InvalidInput });
            var controller = new MessageController(
                loggerMock.Object,
                mediatorMock.Object
            );

            var response = await controller.SaveMessage(new Message());

            Assert.IsType<BadRequestResult>(response.Result);
        }

        [Fact]
        public async void GetMessageMetrics_Should_Return_Ok_Result()
        {

           var mediatorMock = new Mock<IMediator>();
           var loggerMock = new Mock<ILogger>();

           mediatorMock
               .Setup(x => x.Send(It.IsAny<GetMessageMetricsByPeriodSelectionQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new QueryResult<MessageMetrics>() { Type = QueryResultTypeEnum.Success });
           var controller = new MessageController(
               loggerMock.Object,
               mediatorMock.Object
           );

           var response = await controller.GetMessageMetrics(PeriodSelectionEnum.AllTime);

           Assert.IsType<OkObjectResult>(response.Result);
           mediatorMock.Verify(x => x.Send(It.IsAny<GetMessageMetricsByPeriodSelectionQuery>(), It.IsAny<CancellationToken>()), Times.Once());

        }

        [Fact]
        public async void GetMessageMetrics_Should_Return_Not_Found_Result_If_Data_Not_Found()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger>();

            mediatorMock
                .Setup(x => x.Send(It.IsAny<GetMessageMetricsByPeriodSelectionQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new QueryResult<MessageMetrics>() { Type = QueryResultTypeEnum.NotFound });
            var controller = new MessageController(
                loggerMock.Object,
                mediatorMock.Object
            );

            var response = await controller.GetMessageMetrics(PeriodSelectionEnum.AllTime);

            Assert.IsType<NotFoundResult>(response.Result);
        }
    }
}
