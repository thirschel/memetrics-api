using System;
using System.Threading;
using MeMetrics.Application.Helpers;
using MeMetrics.Application.Interfaces;
using MeMetrics.Application.Models;
using MeMetrics.Application.Models.Enums;
using MeMetrics.Application.Queries.Rides;
using MeMetrics.Domain.Models.Rides;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Serilog;
using Xunit;

namespace MeMetrics.Application.Tests.Queries.Ride
{
    public class GetRideMetricsByPeriodSelectionQueryHandlerTests
    {
        [Fact]
        public async void Handle_ShouldReturnMetricsSuccessfully()
        {
            // ARRANGE
            var rideRepositoryMock = new Mock<IRideRepository>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var loggerMock = new Mock<ILogger>();
            rideRepositoryMock
                .Setup(x => x.GetOverviewRideMetrics(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                    It.IsAny<DateTime>())).ReturnsAsync(new RideMetrics());

            var handler = new GetRideMetricsByPeriodSelectionQueryHandler(
                rideRepositoryMock.Object,
                loggerMock.Object,
                memoryCache
            );

            // ACT
            var response = await handler.Handle(new GetRideMetricsByPeriodSelectionQuery(){DatePeriod = PeriodSelectionEnum.ThisYear}, new CancellationToken());


            // ASSERT
            Assert.Equal(QueryResultTypeEnum.Success, response.Type);
            rideRepositoryMock.Verify(x => x.GetOverviewRideMetrics(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public async void Handle_ShouldReturnMetricsFromCache_WhenDatePeriodIsCached()
        {
            // ARRANGE
            var rideRepositoryMock = new Mock<IRideRepository>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var loggerMock = new Mock<ILogger>();

            var datePeriod = PeriodSelectionEnum.ThisYear;
            var dateRange = DatePeriodHelper.GetDateRangeFromPeriodSelection(datePeriod);
            var cacheKey = $"ride-{dateRange.StartDate.ToString()}-{dateRange.EndDate.ToString()}";
            memoryCache.Set(cacheKey, new RideMetrics());

            var handler = new GetRideMetricsByPeriodSelectionQueryHandler(
                rideRepositoryMock.Object,
                loggerMock.Object,
                memoryCache
            );

            // ACT
            var response = await handler.Handle(new GetRideMetricsByPeriodSelectionQuery() { DatePeriod = datePeriod }, new CancellationToken());


            // ASSERT
            Assert.Equal(QueryResultTypeEnum.Success, response.Type);
            rideRepositoryMock.Verify(x => x.GetOverviewRideMetrics(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Fact]
        public async void Handle_ShouldReturnMetricsFromRepository_WhenRefreshCacheTrue()
        {
            // ARRANGE
            var rideRepositoryMock = new Mock<IRideRepository>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var loggerMock = new Mock<ILogger>();
            rideRepositoryMock
                .Setup(x => x.GetOverviewRideMetrics(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                    It.IsAny<DateTime>())).ReturnsAsync(new RideMetrics());

            var datePeriod = PeriodSelectionEnum.ThisYear;
            var dateRange = DatePeriodHelper.GetDateRangeFromPeriodSelection(datePeriod);
            var cacheKey = $"ride-{dateRange.StartDate.ToString()}-{dateRange.EndDate.ToString()}";
            memoryCache.Set(cacheKey, new RideMetrics());

            var handler = new GetRideMetricsByPeriodSelectionQueryHandler(
                rideRepositoryMock.Object,
                loggerMock.Object,
                memoryCache
            );

            // ACT
            var response = await handler.Handle(new GetRideMetricsByPeriodSelectionQuery() { DatePeriod = datePeriod, RefreshCache = true}, new CancellationToken());


            // ASSERT
            Assert.Equal(QueryResultTypeEnum.Success, response.Type);
            rideRepositoryMock.Verify(x => x.GetOverviewRideMetrics(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        }
    }
}
