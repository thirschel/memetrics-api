using Xunit;
using MeMetrics.Application.Helpers;
using System;
using System.Globalization;
using MeMetrics.Application.Models.Enums;

namespace MeMetrics.Application.Tests.Helpers
{
    public class DatePeriodHelpersTests
    {
        [Fact]
        public void GetDateRangeFromPeriodSelection_For_All_Time_Should_Return_Successful() 
        {
            var result = DatePeriodHelper.GetDateRangeFromPeriodSelection(PeriodSelectionEnum.AllTime);

            Assert.Equal(null, result.StartDate);
            Assert.Equal(new DateTime(2100, 1, 1), result.EndDate);

            Assert.Equal(null, result.PreviousPeriodStartDate);
            Assert.Equal(null, result.PreviousPeriodEndDate);

            Assert.Equal("All Time", result.CurrentPeriodLabel);
            Assert.Equal(null, result.PriorPeriodLabel);

        }

        [Fact]
        public void GetDateRangeFromPeriodSelection_For_Last_Year_Should_Return_Successful()
        {

            var result = DatePeriodHelper.GetDateRangeFromPeriodSelection(PeriodSelectionEnum.LastYear);
            var daysBetweenDates = (new DateTime(DateTime.Now.Year, 1, 1) - new DateTime(DateTime.Now.Year - 1, 1, 1)).TotalDays * -1;

            Assert.Equal(new DateTime(DateTime.Now.Year - 1, 1, 1), result.StartDate);
            Assert.Equal(new DateTime(DateTime.Now.Year, 1, 1), result.EndDate);

            Assert.Equal(new DateTime(DateTime.Now.Year - 1, 1, 1).AddDays(daysBetweenDates), result.PreviousPeriodStartDate);
            Assert.Equal(new DateTime(DateTime.Now.Year, 1, 1).AddDays(daysBetweenDates), result.PreviousPeriodEndDate);

            Assert.Equal((DateTime.Now.Year - 2).ToString(), result.PriorPeriodLabel);
            Assert.Equal((DateTime.Now.Year - 1).ToString(), result.CurrentPeriodLabel);
        }

        [Fact]
        public void GetDateRangeFromPeriodSelection_For_This_Year_Should_Return_Successful()
        {
            var result = DatePeriodHelper.GetDateRangeFromPeriodSelection(PeriodSelectionEnum.ThisYear);
            var daysBetweenDates = (new DateTime(DateTime.Now.Year + 1, 1, 1) - new DateTime(DateTime.Now.Year, 1, 1)).TotalDays * -1;

            Assert.Equal(new DateTime(DateTime.Now.Year, 1, 1), result.StartDate);
            Assert.Equal(new DateTime(DateTime.Now.Year + 1, 1, 1), result.EndDate);

            Assert.Equal(new DateTime(DateTime.Now.Year, 1, 1).AddDays(daysBetweenDates), result.PreviousPeriodStartDate);
            Assert.Equal(new DateTime(DateTime.Now.Year + 1, 1, 1).AddDays(daysBetweenDates), result.PreviousPeriodEndDate);

            Assert.Equal((DateTime.Now.Year - 1).ToString(), result.PriorPeriodLabel);
            Assert.Equal((DateTime.Now.Year).ToString(), result.CurrentPeriodLabel);
        }

        [Fact]
        public void GetDateRangeFromPeriodSelection_For_This_Month_Should_Return_Successful()
        {
            var result = DatePeriodHelper.GetDateRangeFromPeriodSelection(PeriodSelectionEnum.ThisMonth);
            var daysBetweenDates = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1) - new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).TotalDays * -1;

            Assert.Equal(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), result.StartDate);
            Assert.Equal(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1), result.EndDate);

            Assert.Equal(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(daysBetweenDates), result.PreviousPeriodStartDate);
            Assert.Equal(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(daysBetweenDates), result.PreviousPeriodEndDate);

            Assert.Equal(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("MMM yyyy", CultureInfo.InvariantCulture), result.PriorPeriodLabel);
            Assert.Equal(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).ToString("MMM yyyy", CultureInfo.InvariantCulture), result.CurrentPeriodLabel);
        }

        [Fact]
        public void GetDateRangeFromPeriodSelection_For_Rolling_30_Should_Return_Successful()
        {
            var result = DatePeriodHelper.GetDateRangeFromPeriodSelection(PeriodSelectionEnum.Rolling30);
            var daysBetweenDates = (new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day) - new DateTime(DateTime.Now.AddDays(-30).Year, DateTime.Now.AddDays(-30).Month, DateTime.Now.AddDays(-30).Day)).TotalDays * -1;

            Assert.Equal(new DateTime(DateTime.Now.AddDays(-30).Year, DateTime.Now.AddDays(-30).Month, DateTime.Now.AddDays(-30).Day), result.StartDate);
            Assert.Equal(new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day), result.EndDate);

            Assert.Equal(new DateTime(DateTime.Now.AddDays(-30).Year, DateTime.Now.AddDays(-30).Month, DateTime.Now.AddDays(-30).Day).AddDays(daysBetweenDates), result.PreviousPeriodStartDate);
            Assert.Equal(new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day).AddDays(daysBetweenDates), result.PreviousPeriodEndDate);

            Assert.Equal($"{new DateTime(DateTime.Now.AddDays(-30).Year, DateTime.Now.AddDays(-30).Month, DateTime.Now.AddDays(-30).Day).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)} - {new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}", result.CurrentPeriodLabel);
            Assert.Equal($"{new DateTime(DateTime.Now.AddDays(-30).Year, DateTime.Now.AddDays(-30).Month, DateTime.Now.AddDays(-30).Day).AddDays(daysBetweenDates).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)} - {new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day).AddDays(daysBetweenDates).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}", result.PriorPeriodLabel);
        }
    }
}
