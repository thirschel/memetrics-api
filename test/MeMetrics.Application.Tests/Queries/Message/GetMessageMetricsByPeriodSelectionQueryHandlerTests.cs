using System;
using System.Threading;
using MeMetrics.Application.Helpers;
using MeMetrics.Application.Interfaces;
using MeMetrics.Application.Models;
using MeMetrics.Application.Models.Enums;
using MeMetrics.Application.Queries.Messages;
using MeMetrics.Domain.Models.Messages;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Serilog;
using Xunit;

namespace MeMetrics.Application.Tests.Queries.Message
{
    public class GetMessageMetricsByPeriodSelectionQueryHandlerTests
    {
        [Fact]
        public async void Handle_ShouldReturnMetricsSuccessfully()
        {
            // ARRANGE
            var messageRepositoryMock = new Mock<IMessageRepository>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var loggerMock = new Mock<ILogger>();
            messageRepositoryMock
                .Setup(x => x.GetOverviewMessageMetrics(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                    It.IsAny<DateTime>())).ReturnsAsync(new MessageMetrics());

            var handler = new GetMessageMetricsByPeriodSelectionQueryHandler(
                messageRepositoryMock.Object,
                loggerMock.Object,
                memoryCache
            );

            // ACT
            var response = await handler.Handle(new GetMessageMetricsByPeriodSelectionQuery(){DatePeriod = PeriodSelectionEnum.ThisYear}, new CancellationToken());


            // ASSERT
            Assert.Equal(QueryResultTypeEnum.Success, response.Type);
            messageRepositoryMock.Verify(x => x.GetOverviewMessageMetrics(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public async void Handle_ShouldReturnMetricsFromCache_WhenDatePeriodIsCached()
        {
            // ARRANGE
            var messageRepositoryMock = new Mock<IMessageRepository>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var loggerMock = new Mock<ILogger>();

            var datePeriod = PeriodSelectionEnum.ThisYear;
            var dateRange = DatePeriodHelper.GetDateRangeFromPeriodSelection(datePeriod);
            var cacheKey = $"message-{dateRange.StartDate.ToString()}-{dateRange.EndDate.ToString()}";
            memoryCache.Set(cacheKey, new MessageMetrics());

            var handler = new GetMessageMetricsByPeriodSelectionQueryHandler(
                messageRepositoryMock.Object,
                loggerMock.Object,
                memoryCache
            );

            // ACT
            var response = await handler.Handle(new GetMessageMetricsByPeriodSelectionQuery() { DatePeriod = datePeriod }, new CancellationToken());


            // ASSERT
            Assert.Equal(QueryResultTypeEnum.Success, response.Type);
            messageRepositoryMock.Verify(x => x.GetOverviewMessageMetrics(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Fact]
        public async void Handle_ShouldReturnMetricsFromRepository_WhenRefreshCacheTrue()
        {
            // ARRANGE
            var messageRepositoryMock = new Mock<IMessageRepository>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var loggerMock = new Mock<ILogger>();
            messageRepositoryMock
                .Setup(x => x.GetOverviewMessageMetrics(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                    It.IsAny<DateTime>())).ReturnsAsync(new MessageMetrics());

            var datePeriod = PeriodSelectionEnum.ThisYear;
            var dateRange = DatePeriodHelper.GetDateRangeFromPeriodSelection(datePeriod);
            var cacheKey = $"message-{dateRange.StartDate.ToString()}-{dateRange.EndDate.ToString()}";
            memoryCache.Set(cacheKey, new MessageMetrics());

            var handler = new GetMessageMetricsByPeriodSelectionQueryHandler(
                messageRepositoryMock.Object,
                loggerMock.Object,
                memoryCache
            );

            // ACT
            var response = await handler.Handle(new GetMessageMetricsByPeriodSelectionQuery() { DatePeriod = datePeriod, RefreshCache = true}, new CancellationToken());


            // ASSERT
            Assert.Equal(QueryResultTypeEnum.Success, response.Type);
            messageRepositoryMock.Verify(x => x.GetOverviewMessageMetrics(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        }
    }
}
