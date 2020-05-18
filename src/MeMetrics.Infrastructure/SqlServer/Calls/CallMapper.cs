using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MeMetrics.Domain.Models.Calls;
using MeMetrics.Infrastructure.SqlServer.Entities;

namespace MeMetrics.Infrastructure.SqlServer.Calls
{
    public class CallMapper : ICallMapper
    {
        private readonly IMapper _mapper;

        public CallMapper(
            IMapper mapper)
        {
            _mapper = mapper;
        }

        public List<CallByDayOfWeek> BuildCallsByDayOfWeek(List<PerDayOfWeekEntity> entities)
        {
            var baseByDayOfWeekEntities = DayOfTheWeekHelper.GenerateBaseDayOfWeekDtos();
            foreach (var perDayOfWeekDto in entities.GroupBy(m => m.DayOfWeek))
            {
                var index = baseByDayOfWeekEntities.FindIndex(d => d.DayOfWeek == perDayOfWeekDto.First().DayOfWeek);
                baseByDayOfWeekEntities[index].IncomingTotal = perDayOfWeekDto.FirstOrDefault(i => i.IsIncoming)?.Total;
                baseByDayOfWeekEntities[index].OutgoingTotal = perDayOfWeekDto.FirstOrDefault(i => !i.IsIncoming)?.Total;
            }

            return _mapper.Map<List<CallByDayOfWeek>>(baseByDayOfWeekEntities);
        }

        public List<CallByHour> BuildCallsByHour(List<PerHourEntity> entities)
        {
            var baseByHourEntities = PerHourHelper.GenerateBasePerHourDtos();
            foreach (var perHourDto in entities.GroupBy(m => new { m.Day, m.Hour }))
            {
                var index = baseByHourEntities.FindIndex(d => d.Day == perHourDto.Key.Day && d.Hour == perHourDto.Key.Hour);
                baseByHourEntities[index].IncomingTotal = perHourDto.FirstOrDefault(i => i.IsIncoming)?.Total;
                baseByHourEntities[index].OutgoingTotal = perHourDto.FirstOrDefault(i => !i.IsIncoming)?.Total;
                baseByHourEntities[index].Total = perHourDto.Sum(g => g.Total);
            }

            return _mapper.Map<List<CallByHour>>(baseByHourEntities);
        }

        public List<CallByPeriod> BuildCallsByPeriod(List<DateTime> calendarDays, List<PerformanceEntity> callsPerformance, DateTime previousPeriodEndDate)
        {
            var callPerformance = new List<CallByPeriod>();
            if (!callsPerformance.Any())
            {
                return callPerformance;
            }
            var priorPeriodDays = calendarDays.Where(d => d < previousPeriodEndDate).ToList();
            var currentPeriodDays = calendarDays.Where(d => d > previousPeriodEndDate && d <= DateTime.Today).ToList();
            for (var i = 0; i < priorPeriodDays.Count(); i++)
            {
                var performanceEntity = callsPerformance.FirstOrDefault(m => m.OccurredDate == priorPeriodDays[i]);
                callPerformance.Add(new CallByPeriod()
                {
                    DayNumber = i + 1,
                    IsPreviousPeriod = true,
                    Count = performanceEntity?.Count ?? 0
                });
            }
            for (var i = 0; i < currentPeriodDays.Count(); i++)
            {
                var performanceEntity = callsPerformance.FirstOrDefault(m => m.OccurredDate == currentPeriodDays[i]);
                callPerformance.Add(new CallByPeriod()
                {
                    DayNumber = i + 1,
                    IsPreviousPeriod = false,
                    Count = performanceEntity?.Count ?? 0
                });
            }

            return callPerformance;
        }
    }
}
