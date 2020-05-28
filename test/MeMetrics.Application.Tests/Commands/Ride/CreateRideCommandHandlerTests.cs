using System.Threading;
using MeMetrics.Application.Commands.Ride;
using MeMetrics.Application.Interfaces;
using MeMetrics.Application.Models;
using Moq;
using Serilog;
using Xunit;

namespace MeMetrics.Application.Tests.Commands.Ride
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
           var response = await handler.Handle(new CreateRideCommand() { Ride = new Domain.Models.Rides.Ride() }, new CancellationToken());

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

            rideRepositoryMock.Setup(x => x.InsertRide(It.IsAny<Domain.Models.Rides.Ride>())).ReturnsAsync(1);

            // ACT
            var response = await handler.Handle(new CreateRideCommand(){Ride = new Domain.Models.Rides.Ride() { RideId = "1" }}, new CancellationToken());

           // ASSERT
           Assert.Equal(CommandResultTypeEnum.Success, response.Type);
            rideRepositoryMock.Verify(x => x.InsertRide(It.IsAny<Domain.Models.Rides.Ride>()), Times.Once);
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

            rideRepositoryMock.Setup(x => x.InsertRide(It.IsAny<Domain.Models.Rides.Ride>())).ReturnsAsync(0);

            // ACT
            var response = await handler.Handle(new CreateRideCommand() { Ride = new Domain.Models.Rides.Ride() { RideId = "1" } }, new CancellationToken());

            // ASSERT
            Assert.Equal(CommandResultTypeEnum.UnprocessableEntity, response.Type);
            rideRepositoryMock.Verify(x => x.InsertRide(It.IsAny<Domain.Models.Rides.Ride>()), Times.Once);
        }
    }
}
