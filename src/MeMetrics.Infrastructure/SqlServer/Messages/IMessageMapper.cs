using System;
using System.Collections.Generic;
using MeMetrics.Domain.Models.Messages;
using MeMetrics.Infrastructure.SqlServer.Entities;

namespace MeMetrics.Infrastructure.SqlServer.Messages
{
    public interface IMessageMapper
    {
        int GetTotalMessagesByGender(List<MessageByGenderEntity> entities, string gender);
        int GetAverageTextLengthByGender(List<MessageByGenderEntity> entities, string gender);
        List<MessageByDayOfWeek> BuildMessagesByDayOfWeek(List<PerDayOfWeekEntity> entities);
        List<MessageByHour> BuildMessagesByHour(List<PerHourEntity> entities);
        List<MessageByPeriod> BuildMessagesByPeriod(List<DateTime> calendarDays, List<PerformanceEntity> callsPerformance, DateTime previousPeriodEndDate);
    }
}
