using System.Threading;
using MeMetrics.Application.Models;
using MeMetrics.Application.Interfaces;
using Moq;
using Serilog;
using Xunit;
using MeMetrics.Application.Commands.Ride;
using MeMetrics.Domain.Models.Rides;

namespace MeMetrics.Application.Tests.Commands.Example
{
    public class CreateRideCommandHandlerTests
    {
        [Fact]
        public async void Request_With_No_Id_Should_Return_Invalid_Input()
        {
           // ARRANGE
           var validator = new CreateRideCommandValidator();
           var rideRepositoryMock = new Mock<IRideRepository>();
           var loggerMock = new Mock<ILogger>();

           var handler = new CreateRideCommandHandler(
               rideRepositoryMock.Object,
               loggerMock.Object,
               validator
           );

           // ACT
           var response = await handler.Handle(new CreateRideCommand() { Ride = new Ride() }, new CancellationToken());

           // ASSERT
           Assert.Equal(CommandResultTypeEnum.InvalidInput, response.Type);
        }

        [Fact]
        public async void Should_Ride_InsertRide_In_Repository()
        {
            // ARRANGE
            var validator = new CreateRideCommandValidator();
            var rideRepositoryMock = new Mock<IRideRepository>();
            var loggerMock = new Mock<ILogger>();

            var handler = new CreateRideCommandHandler(
                rideRepositoryMock.Object,
                loggerMock.Object,
                validator
            );

            rideRepositoryMock.Setup(x => x.InsertRide(It.IsAny<Ride>())).ReturnsAsync(1);

            // ACT
            var response = await handler.Handle(new CreateRideCommand(){Ride = new Ride() { RideId = "1" }}, new CancellationToken());

           // ASSERT
           Assert.Equal(CommandResultTypeEnum.Success, response.Type);
            rideRepositoryMock.Verify(x => x.InsertRide(It.IsAny<Ride>()), Times.Once);
        }

        [Fact]
        public async void Should_Return_Unprocessable_If_No_Rows_Affected()
        {
            // ARRANGE
            var validator = new CreateRideCommandValidator();
            var rideRepositoryMock = new Mock<IRideRepository>();
            var loggerMock = new Mock<ILogger>();

            var handler = new CreateRideCommandHandler(
                rideRepositoryMock.Object,
                loggerMock.Object,
                validator
            );

            rideRepositoryMock.Setup(x => x.InsertRide(It.IsAny<Ride>())).ReturnsAsync(0);

            // ACT
            var response = await handler.Handle(new CreateRideCommand() { Ride = new Ride() { RideId = "1" } }, new CancellationToken());

            // ASSERT
            Assert.Equal(CommandResultTypeEnum.UnprocessableEntity, response.Type);
            rideRepositoryMock.Verify(x => x.InsertRide(It.IsAny<Ride>()), Times.Once);
        }
    }
}
