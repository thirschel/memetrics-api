using System;
using System.Threading;
using MeMetrics.Application.Helpers;
using MeMetrics.Application.Interfaces;
using MeMetrics.Application.Models;
using MeMetrics.Application.Models.Enums;
using MeMetrics.Application.Queries.Call;
using MeMetrics.Domain.Models.Calls;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Serilog;
using Xunit;

namespace MeMetrics.Application.Tests.Queries.Call
{
    public class GetCallMetricsByPeriodSelectionQueryHandlerTests
    {
        [Fact]
        public async void Handle_ShouldReturnMetricsSuccessfully()
        {
            // ARRANGE
            var callRepositoryMock = new Mock<ICallRepository>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var loggerMock = new Mock<ILogger>();
            callRepositoryMock
                .Setup(x => x.GetOverviewCallMetrics(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                    It.IsAny<DateTime>())).ReturnsAsync(new CallMetrics());

            var handler = new GetCallMetricsByPeriodSelectionQueryHandler(
                callRepositoryMock.Object,
                loggerMock.Object,
                memoryCache
            );

            // ACT
            var response = await handler.Handle(new GetCallMetricsByPeriodSelectionQuery(){DatePeriod = PeriodSelectionEnum.ThisYear}, new CancellationToken());


            // ASSERT
            Assert.Equal(QueryResultTypeEnum.Success, response.Type);
            callRepositoryMock.Verify(x => x.GetOverviewCallMetrics(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public async void Handle_ShouldReturnMetricsFromCache_WhenDatePeriodIsCached()
        {
            // ARRANGE
            var callRepositoryMock = new Mock<ICallRepository>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var loggerMock = new Mock<ILogger>();

            var datePeriod = PeriodSelectionEnum.ThisYear;
            var dateRange = DatePeriodHelper.GetDateRangeFromPeriodSelection(datePeriod);
            var cacheKey = $"call-{dateRange.StartDate.ToString()}-{dateRange.EndDate.ToString()}";
            memoryCache.Set(cacheKey, new CallMetrics());

            var handler = new GetCallMetricsByPeriodSelectionQueryHandler(
                callRepositoryMock.Object,
                loggerMock.Object,
                memoryCache
            );

            // ACT
            var response = await handler.Handle(new GetCallMetricsByPeriodSelectionQuery() { DatePeriod = datePeriod }, new CancellationToken());


            // ASSERT
            Assert.Equal(QueryResultTypeEnum.Success, response.Type);
            callRepositoryMock.Verify(x => x.GetOverviewCallMetrics(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Fact]
        public async void Handle_ShouldReturnMetricsFromRepository_WhenRefreshCacheTrue()
        {
            // ARRANGE
            var callRepositoryMock = new Mock<ICallRepository>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var loggerMock = new Mock<ILogger>();
            callRepositoryMock
                .Setup(x => x.GetOverviewCallMetrics(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                    It.IsAny<DateTime>())).ReturnsAsync(new CallMetrics());

            var datePeriod = PeriodSelectionEnum.ThisYear;
            var dateRange = DatePeriodHelper.GetDateRangeFromPeriodSelection(datePeriod);
            var cacheKey = $"call-{dateRange.StartDate.ToString()}-{dateRange.EndDate.ToString()}";
            memoryCache.Set(cacheKey, new CallMetrics());

            var handler = new GetCallMetricsByPeriodSelectionQueryHandler(
                callRepositoryMock.Object,
                loggerMock.Object,
                memoryCache
            );

            // ACT
            var response = await handler.Handle(new GetCallMetricsByPeriodSelectionQuery() { DatePeriod = datePeriod, RefreshCache = true}, new CancellationToken());


            // ASSERT
            Assert.Equal(QueryResultTypeEnum.Success, response.Type);
            callRepositoryMock.Verify(x => x.GetOverviewCallMetrics(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        }
    }
}
