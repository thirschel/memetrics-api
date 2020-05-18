using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MeMetrics.Application.Models;
using Moq;
using Serilog;
using Xunit;
using MeMetrics.Api.Controllers;
using MeMetrics.Application.Commands.Ride;
using MeMetrics.Application.Models.Enums;
using MeMetrics.Domain.Models.Rides;
using MeMetrics.Application.Queries.Rides;

namespace MeMetrics.Api.Tests
{
    public class RideControllerTests
    {
        [Fact]
        public async void SaveRide_Should_Return_Ok_Result()
        {
           var mediatorMock = new Mock<IMediator>();
           var loggerMock = new Mock<ILogger>();

           mediatorMock
               .Setup(x => x.Send(It.IsAny<CreateRideCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new CommandResult<bool>(){ Type = CommandResultTypeEnum.Success });
           var controller = new RideController(
               loggerMock.Object,
               mediatorMock.Object
           );

           var response = await controller.SaveRide(new Ride());

           Assert.IsType<OkObjectResult>(response.Result);
           mediatorMock.Verify(x => x.Send(It.IsAny<CreateRideCommand>(), It.IsAny<CancellationToken>()), Times.Once());

        }

        [Fact]
        public async void SaveRide_Should_Return_Bad_Result_If_Invalid_Input()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger>();

            mediatorMock
                .Setup(x => x.Send(It.IsAny<CreateRideCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CommandResult<bool>() { Type = CommandResultTypeEnum.InvalidInput });
            var controller = new RideController(
                loggerMock.Object,
                mediatorMock.Object
            );

            var response = await controller.SaveRide(new Ride());

            Assert.IsType<BadRequestResult>(response.Result);
        }

        [Fact]
        public async void GetRideMetrics_Should_Return_Ok_Result()
        {

           var mediatorMock = new Mock<IMediator>();
           var loggerMock = new Mock<ILogger>();

           mediatorMock
               .Setup(x => x.Send(It.IsAny<GetRideMetricsByPeriodSelectionQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new QueryResult<RideMetrics>() { Type = QueryResultTypeEnum.Success });
           var controller = new RideController(
               loggerMock.Object,
               mediatorMock.Object
           );

           var response = await controller.GetRideMetrics(PeriodSelectionEnum.AllTime);

           Assert.IsType<OkObjectResult>(response.Result);
           mediatorMock.Verify(x => x.Send(It.IsAny<GetRideMetricsByPeriodSelectionQuery>(), It.IsAny<CancellationToken>()), Times.Once());

        }

        [Fact]
        public async void GetRideMetrics_Should_Return_Not_Found_Result_If_Data_Not_Found()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger>();

            mediatorMock
                .Setup(x => x.Send(It.IsAny<GetRideMetricsByPeriodSelectionQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new QueryResult<RideMetrics>() { Type = QueryResultTypeEnum.NotFound });
            var controller = new RideController(
                loggerMock.Object,
                mediatorMock.Object
            );

            var response = await controller.GetRideMetrics(PeriodSelectionEnum.AllTime);

            Assert.IsType<NotFoundResult>(response.Result);
        }
    }
}
