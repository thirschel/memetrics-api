using System;
using System.Threading;
using MeMetrics.Application.Helpers;
using MeMetrics.Application.Interfaces;
using MeMetrics.Application.Models;
using MeMetrics.Application.Models.Enums;
using MeMetrics.Application.Queries.RecruitmentMessages;
using MeMetrics.Domain.Models.RecruitmentMessage;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Serilog;
using Xunit;

namespace MeMetrics.Application.Tests.Queries.RecruitmentMessage
{
    public class GetRecruitmentMessageMetricsByPeriodSelectionQueryHandlerTests
    {
        [Fact]
        public async void Handle_ShouldReturnMetricsSuccessfully()
        {
            // ARRANGE
            var recruitmentMessageRepositoryMock = new Mock<IRecruitmentMessageRepository>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var loggerMock = new Mock<ILogger>();
            recruitmentMessageRepositoryMock
                .Setup(x => x.GetOverviewRecruitmentMessageMetrics(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                    It.IsAny<DateTime>())).ReturnsAsync(new RecruitmentMessageMetrics());

            var handler = new GetRecruitmentMessageMetricsByPeriodSelectionQueryHandler(
                recruitmentMessageRepositoryMock.Object,
                loggerMock.Object,
                memoryCache
            );

            // ACT
            var response = await handler.Handle(new GetRecruitmentMessageMetricsByPeriodSelectionQuery(){DatePeriod = PeriodSelectionEnum.ThisYear}, new CancellationToken());


            // ASSERT
            Assert.Equal(QueryResultTypeEnum.Success, response.Type);
            recruitmentMessageRepositoryMock.Verify(x => x.GetOverviewRecruitmentMessageMetrics(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public async void Handle_ShouldReturnMetricsFromCache_WhenDatePeriodIsCached()
        {
            // ARRANGE
            var recruitmentMessageRepositoryMock = new Mock<IRecruitmentMessageRepository>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var loggerMock = new Mock<ILogger>();

            var datePeriod = PeriodSelectionEnum.ThisYear;
            var dateRange = DatePeriodHelper.GetDateRangeFromPeriodSelection(datePeriod);
            var cacheKey = $"recruitment-message-{dateRange.StartDate.ToString()}-{dateRange.EndDate.ToString()}";
            memoryCache.Set(cacheKey, new RecruitmentMessageMetrics());

            var handler = new GetRecruitmentMessageMetricsByPeriodSelectionQueryHandler(
                recruitmentMessageRepositoryMock.Object,
                loggerMock.Object,
                memoryCache
            );

            // ACT
            var response = await handler.Handle(new GetRecruitmentMessageMetricsByPeriodSelectionQuery() { DatePeriod = datePeriod }, new CancellationToken());


            // ASSERT
            Assert.Equal(QueryResultTypeEnum.Success, response.Type);
            recruitmentMessageRepositoryMock.Verify(x => x.GetOverviewRecruitmentMessageMetrics(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Fact]
        public async void Handle_ShouldReturnMetricsFromRepository_WhenRefreshCacheTrue()
        {
            // ARRANGE
            var recruitmentMessageRepositoryMock = new Mock<IRecruitmentMessageRepository>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var loggerMock = new Mock<ILogger>();
            recruitmentMessageRepositoryMock
                .Setup(x => x.GetOverviewRecruitmentMessageMetrics(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                    It.IsAny<DateTime>())).ReturnsAsync(new RecruitmentMessageMetrics());

            var datePeriod = PeriodSelectionEnum.ThisYear;
            var dateRange = DatePeriodHelper.GetDateRangeFromPeriodSelection(datePeriod);
            var cacheKey = $"recruitmentMessage-{dateRange.StartDate.ToString()}-{dateRange.EndDate.ToString()}";
            memoryCache.Set(cacheKey, new RecruitmentMessageMetrics());

            var handler = new GetRecruitmentMessageMetricsByPeriodSelectionQueryHandler(
                recruitmentMessageRepositoryMock.Object,
                loggerMock.Object,
                memoryCache
            );

            // ACT
            var response = await handler.Handle(new GetRecruitmentMessageMetricsByPeriodSelectionQuery() { DatePeriod = datePeriod, RefreshCache = true}, new CancellationToken());


            // ASSERT
            Assert.Equal(QueryResultTypeEnum.Success, response.Type);
            recruitmentMessageRepositoryMock.Verify(x => x.GetOverviewRecruitmentMessageMetrics(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        }
    }
}
