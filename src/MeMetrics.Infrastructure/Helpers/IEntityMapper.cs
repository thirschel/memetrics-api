using System;
using System.Collections.Generic;
using MeMetrics.Domain.Models;
using MeMetrics.Infrastructure.SqlServer.Entities;
using MeMetrics.Infrastructure.SqlServer.Messages;

namespace MeMetrics.Infrastructure.Helpers
{
    public interface IEntityMapper
    {
        int GetTotalMessagesByGender(List<MessageByGenderEntity> entities, string gender);
        int GetAverageTextLengthByGender(List<MessageByGenderEntity> entities, string gender);
        List<T> BuildByDayOfWeek<T>(List<PerDayOfWeekEntity> entities);
        List<T> BuildByHour<T>(List<PerHourEntity> entities);
        List<ByPeriod> BuildByPeriod(List<DateTime> calendarDays, List<PerformanceEntity> callsPerformance, DateTime? previousPeriodEndDate);
    }
}
