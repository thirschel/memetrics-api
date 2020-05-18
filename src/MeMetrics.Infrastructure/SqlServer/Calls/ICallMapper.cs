using System;
using System.Collections.Generic;
using MeMetrics.Domain.Models;
using MeMetrics.Domain.Models.Calls;
using MeMetrics.Infrastructure.SqlServer.Entities;

namespace MeMetrics.Infrastructure.SqlServer.Calls
{
    public interface ICallMapper
    {
        List<CallByDayOfWeek> BuildCallsByDayOfWeek(List<PerDayOfWeekEntity> entities);
        List<CallByHour> BuildCallsByHour(List<PerHourEntity> entities);
        List<CallByPeriod> BuildCallsByPeriod(List<DateTime> calendarDays, List<PerformanceEntity> callsPerformance, DateTime previousPeriodEndDate);
    }
}
