using System;
using System.Collections.Generic;
using MeMetrics.Domain.Models.RecruitmentMessage;
using MeMetrics.Infrastructure.SqlServer.Entities;

namespace MeMetrics.Infrastructure.SqlServer.RecruitmentMessages
{
    public interface IRecruitmentMessageMapper
    {
        List<RecruitmentMessageByDayOfWeek> BuildRecruitmentMessagesByDayOfWeek(List<PerDayOfWeekEntity> entities);
        List<RecruitmentMessageByPeriod> BuildRecruitmentMessagesByPeriod(List<DateTime> calendarDays, List<PerformanceEntity> callsPerformance, DateTime previousPeriodEndDate);
    }
}
