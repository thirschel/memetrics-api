using System;
using System.Collections.Generic;
using MeMetrics.Domain.Models.Rides;
using MeMetrics.Infrastructure.SqlServer.Entities;

namespace MeMetrics.Infrastructure.SqlServer.Rides
{
    public interface IRideMapper
    {
        List<RideByDayOfWeek> BuildCallsByDayOfWeek(List<PerDayOfWeekEntity> entities);
        List<RideByHour> BuildCallsByHour(List<PerHourEntity> entities);
        List<RideByPeriod> BuildCallsByPeriod(List<DateTime> calendarDays, List<PerformanceEntity> callsPerformance, DateTime previousPeriodEndDate);
    }
}
