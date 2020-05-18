using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MeMetrics.Domain.Models.RecruitmentMessage;
using MeMetrics.Infrastructure.SqlServer.Entities;

namespace MeMetrics.Infrastructure.SqlServer.RecruitmentMessages
{
    public class RecruitmentMessageMapper : IRecruitmentMessageMapper
    {
        private readonly IMapper _mapper;

        public RecruitmentMessageMapper(
            IMapper mapper)
        {
            _mapper = mapper;
        }

        public List<RecruitmentMessageByDayOfWeek> BuildRecruitmentMessagesByDayOfWeek(List<PerDayOfWeekEntity> entities)
        {
            var baseByDayOfWeekEntities = DayOfTheWeekHelper.GenerateBaseDayOfWeekDtos();
            foreach (var perDayOfWeekDto in entities.GroupBy(m => m.DayOfWeek))
            {
                var index = baseByDayOfWeekEntities.FindIndex(d => d.DayOfWeek == perDayOfWeekDto.First().DayOfWeek);
                baseByDayOfWeekEntities[index].IncomingTotal = perDayOfWeekDto.FirstOrDefault(i => i.IsIncoming)?.Total;
                baseByDayOfWeekEntities[index].OutgoingTotal = perDayOfWeekDto.FirstOrDefault(i => !i.IsIncoming)?.Total;
            }

            return _mapper.Map<List<RecruitmentMessageByDayOfWeek>>(baseByDayOfWeekEntities);
        }

        public List<RecruitmentMessageByPeriod> BuildRecruitmentMessagesByPeriod(List<DateTime> calendarDays, List<PerformanceEntity> callsPerformance, DateTime previousPeriodEndDate)
        {
            var callPerformance = new List<RecruitmentMessageByPeriod>();
            if (callsPerformance.Any())
            {
                var priorPeriodDays = calendarDays.Where(d => d < previousPeriodEndDate).ToList();
                var currentPeriodDays = calendarDays.Where(d => d > previousPeriodEndDate && d <= DateTime.Today).ToList();
                for (var i = 0; i < priorPeriodDays.Count(); i++)
                {
                    var count = callsPerformance.FirstOrDefault(m => m.OccurredDate == priorPeriodDays[i]);
                    callPerformance.Add(new RecruitmentMessageByPeriod()
                    {
                        DayNumber = i + 1,
                        IsPreviousPeriod = true,
                        Count = count?.Count ?? 0
                    });
                }
                for (var i = 0; i < currentPeriodDays.Count(); i++)
                {
                    var count = callsPerformance.FirstOrDefault(m => m.OccurredDate == currentPeriodDays[i]);
                    callPerformance.Add(new RecruitmentMessageByPeriod()
                    {
                        DayNumber = i + 1,
                        IsPreviousPeriod = false,
                        Count = count?.Count ?? 0
                    });
                }
            }

            return callPerformance;
        }
    }
}
