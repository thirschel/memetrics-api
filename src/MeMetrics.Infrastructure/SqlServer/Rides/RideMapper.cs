using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MeMetrics.Domain.Models.Rides;
using MeMetrics.Infrastructure.SqlServer.Entities;

namespace MeMetrics.Infrastructure.SqlServer.Rides
{
    public class RideMapper : IRideMapper
    {
        private readonly IMapper _mapper;

        public RideMapper(
            IMapper mapper)
        {
            _mapper = mapper;
        }

        public List<RideByDayOfWeek> BuildCallsByDayOfWeek(List<PerDayOfWeekEntity> entities)
        {
            var baseByDayOfWeekEntities = DayOfTheWeekHelper.GenerateBaseDayOfWeekDtos();
            foreach (var perDayOfWeekDto in entities.GroupBy(m => m.DayOfWeek))
            {
                var index = baseByDayOfWeekEntities.FindIndex(d => d.DayOfWeek == perDayOfWeekDto.First().DayOfWeek);
                baseByDayOfWeekEntities[index].IncomingTotal = perDayOfWeekDto.FirstOrDefault(i => i.IsIncoming)?.Total;
                baseByDayOfWeekEntities[index].OutgoingTotal = perDayOfWeekDto.FirstOrDefault(i => !i.IsIncoming)?.Total;
            }

            return _mapper.Map<List<RideByDayOfWeek>>(baseByDayOfWeekEntities);
        }

        public List<RideByHour> BuildCallsByHour(List<PerHourEntity> entities)
        {
            var baseByHourEntities = PerHourHelper.GenerateBasePerHourDtos();
            foreach (var perHourDto in entities.GroupBy(m => new { m.Day, m.Hour }))
            {
                var index = baseByHourEntities.FindIndex(d => d.Day == perHourDto.Key.Day && d.Hour == perHourDto.Key.Hour);
                baseByHourEntities[index].IncomingTotal = perHourDto.FirstOrDefault(i => i.IsIncoming)?.Total;
                baseByHourEntities[index].OutgoingTotal = perHourDto.FirstOrDefault(i => !i.IsIncoming)?.Total;
                baseByHourEntities[index].Total = perHourDto.Sum(g => g.Total);
            }

            return _mapper.Map<List<RideByHour>>(baseByHourEntities);
        }

        public List<RideByPeriod> BuildCallsByPeriod(List<DateTime> calendarDays, List<PerformanceEntity> callsPerformance, DateTime previousPeriodEndDate)
        {
            var callPerformance = new List<RideByPeriod>();
            if (callsPerformance.Any())
            {
                var priorPeriodDays = calendarDays.Where(d => d < previousPeriodEndDate).ToList();
                var currentPeriodDays = calendarDays.Where(d => d > previousPeriodEndDate && d <= DateTime.Today).ToList();
                for (var i = 0; i < priorPeriodDays.Count(); i++)
                {
                    var count = callsPerformance.FirstOrDefault(m => m.OccurredDate == priorPeriodDays[i]);
                    callPerformance.Add(new RideByPeriod()
                    {
                        DayNumber = i + 1,
                        IsPreviousPeriod = true,
                        Count = count?.Count ?? 0
                    });
                }
                for (var i = 0; i < currentPeriodDays.Count(); i++)
                {
                    var count = callsPerformance.FirstOrDefault(m => m.OccurredDate == currentPeriodDays[i]);
                    callPerformance.Add(new RideByPeriod()
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
