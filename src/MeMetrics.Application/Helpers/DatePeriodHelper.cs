using System;
using System.Globalization;
using MeMetrics.Application.Models;
using MeMetrics.Application.Models.Enums;
using MeMetrics.Objects;

namespace MeMetrics.Application.Helpers
{
    public class DatePeriodHelper
    {
        public static DateRange GetDateRangeFromPeriodSelection(PeriodSelectionEnum periodSelection)
        {
            DateRange dateRange = null;
            switch (periodSelection)
            {
                case PeriodSelectionEnum.AllTime:
                    return new DateRange()
                    {
                        EndDate = new DateTime(2100, 1, 1),
                        CurrentPeriodLabel = "All Time"
                    };
                case PeriodSelectionEnum.LastYear:
                    dateRange = new DateRange()
                    {
                        StartDate = new DateTime(DateTime.Now.Year - 1, 1, 1),
                        EndDate = new DateTime(DateTime.Now.Year, 1, 1),
                        PriorPeriodLabel = (DateTime.Now.Year - 2).ToString(),
                        CurrentPeriodLabel = (DateTime.Now.Year - 1).ToString()
                    };
                    break;
                case PeriodSelectionEnum.ThisYear:
                    dateRange = new DateRange()
                    {
                        StartDate = new DateTime(DateTime.Now.Year, 1, 1),
                        EndDate = new DateTime(DateTime.Now.Year + 1, 1, 1),
                        PriorPeriodLabel = (DateTime.Now.Year - 1).ToString(),
                        CurrentPeriodLabel = (DateTime.Now.Year).ToString()
                    };
                    break;
                case PeriodSelectionEnum.Rolling30:
                    var rollingStart = DateTime.Now.AddDays(-30);
                    var rollingEnd = DateTime.Now.AddDays(1);
                    dateRange = new DateRange()
                    {
                        StartDate = new DateTime(rollingStart.Year, rollingStart.Month, rollingStart.Day),
                        EndDate = new DateTime(rollingEnd.Year, rollingEnd.Month, rollingEnd.Day),
                        CurrentPeriodLabel = $"{rollingStart.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)} -" +
                                             $" {rollingEnd.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}"

                    };
                    break;
                case PeriodSelectionEnum.ThisMonth:
                    var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    var endDate = startDate.AddMonths(1);
                    dateRange = new DateRange()
                    {
                        StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                        EndDate = new DateTime(endDate.Year, endDate.Month, endDate.Day),
                        PriorPeriodLabel = startDate.ToString("MMM yyyy", CultureInfo.InvariantCulture),
                        CurrentPeriodLabel = endDate.ToString("MMM yyyy", CultureInfo.InvariantCulture)
                    };
                    break;
            }
            var daysBetweenDates = (dateRange.EndDate - dateRange.StartDate).Value.TotalDays * -1;
            dateRange.PreviousPeriodStartDate = dateRange.StartDate.Value.AddDays(daysBetweenDates);
            dateRange.PreviousPeriodEndDate = dateRange.EndDate.AddDays(daysBetweenDates);
            if (periodSelection == PeriodSelectionEnum.Rolling30)
            {
                dateRange.PriorPeriodLabel =
                    $"{dateRange.PreviousPeriodStartDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)} -" +
                    $" {dateRange.PreviousPeriodEndDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}";
            }
            return dateRange;
        }
    }
}
