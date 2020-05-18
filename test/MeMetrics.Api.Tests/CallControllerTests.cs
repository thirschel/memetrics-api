using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MeMetrics.Application.Models;
using Moq;
using Serilog;
using Xunit;
using MeMetrics.Application.Commands.Call;
using MeMetrics.Api.Controllers;
using MeMetrics.Application.Models.Enums;
using MeMetrics.Domain.Models.Calls;
using MeMetrics.Application.Queries.Call;

namespace MeMetrics.Api.Tests
{
    public class CallControllerTests
    {
        [Fact]
        public async void SaveCall_Should_Return_Ok_Result()
        {
           var mediatorMock = new Mock<IMediator>();
           var loggerMock = new Mock<ILogger>();

           mediatorMock
               .Setup(x => x.Send(It.IsAny<CreateCallCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new CommandResult<bool>(){ Type = CommandResultTypeEnum.Success });
           var controller = new CallController(
               loggerMock.Object,
               mediatorMock.Object
           );

           var response = await controller.SaveCall(new Call());

           Assert.IsType<OkObjectResult>(response.Result);
           mediatorMock.Verify(x => x.Send(It.IsAny<CreateCallCommand>(), It.IsAny<CancellationToken>()), Times.Once());

        }

        [Fact]
        public async void SaveCall_Should_Return_Bad_Result_If_Invalid_Input()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger>();

            mediatorMock
                .Setup(x => x.Send(It.IsAny<CreateCallCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CommandResult<bool>() { Type = CommandResultTypeEnum.InvalidInput });
            var controller = new CallController(
                loggerMock.Object,
                mediatorMock.Object
            );

            var response = await controller.SaveCall(new Call());

            Assert.IsType<BadRequestResult>(response.Result);
        }

        [Fact]
        public async void GetCallMetrics_Should_Return_Ok_Result()
        {

           var mediatorMock = new Mock<IMediator>();
           var loggerMock = new Mock<ILogger>();

           mediatorMock
               .Setup(x => x.Send(It.IsAny<GetCallMetricsByPeriodSelectionQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new QueryResult<CallMetrics>() { Type = QueryResultTypeEnum.Success });
           var controller = new CallController(
               loggerMock.Object,
               mediatorMock.Object
           );

           var response = await controller.GetCallMetrics(PeriodSelectionEnum.AllTime);

           Assert.IsType<OkObjectResult>(response.Result);
           mediatorMock.Verify(x => x.Send(It.IsAny<GetCallMetricsByPeriodSelectionQuery>(), It.IsAny<CancellationToken>()), Times.Once());

        }

        [Fact]
        public async void GetCallMetrics_Should_Return_Not_Found_Result_If_Data_Not_Found()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger>();

            mediatorMock
                .Setup(x => x.Send(It.IsAny<GetCallMetricsByPeriodSelectionQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new QueryResult<CallMetrics>() { Type = QueryResultTypeEnum.NotFound });
            var controller = new CallController(
                loggerMock.Object,
                mediatorMock.Object
            );

            var response = await controller.GetCallMetrics(PeriodSelectionEnum.AllTime);

            Assert.IsType<NotFoundResult>(response.Result);
        }
    }
}
