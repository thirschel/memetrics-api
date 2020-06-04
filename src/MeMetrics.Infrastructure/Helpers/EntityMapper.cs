using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MeMetrics.Domain.Models;
using MeMetrics.Infrastructure.SqlServer.Entities;
using MeMetrics.Infrastructure.SqlServer.Messages;

namespace MeMetrics.Infrastructure.Helpers
{
    public class EntityMapper: IEntityMapper
    {
        private readonly IMapper _mapper;

        public EntityMapper(
            IMapper mapper)
        {
            _mapper = mapper;
        }

        public int GetTotalMessagesByGender(List<MessageByGenderEntity> entities, string gender)
        {
            var genderEntities = entities.Where(e => e.Gender == gender).ToList();
            return genderEntities.Any() ? entities.Where(e => e.Gender == gender).Sum(e => e.Total) : 0;
        }

        public int GetAverageTextLengthByGender(List<MessageByGenderEntity> entities, string gender)
        {
            var genderEntities = entities.Where(e => e.Gender == gender && !e.IsIncoming).ToList();
            return genderEntities.Any() ? (int) genderEntities.Average(e => e.AverageTextLength) : 0;
        }

        public List<T> BuildByDayOfWeek<T>(List<PerDayOfWeekEntity> entities)
        {
            var baseByDayOfWeekEntities = SqlServer.Entities.DayOfTheWeekHelper.GenerateBaseDayOfWeekDtos();
            foreach (var perDayOfWeekDto in entities.GroupBy(m => m.DayOfWeek))
            {
                var index = baseByDayOfWeekEntities.FindIndex(d => d.DayOfWeek == perDayOfWeekDto.First().DayOfWeek);
                baseByDayOfWeekEntities[index].IncomingTotal = perDayOfWeekDto.FirstOrDefault(i => i.IsIncoming)?.Total ?? 0;
                baseByDayOfWeekEntities[index].OutgoingTotal = perDayOfWeekDto.FirstOrDefault(i => !i.IsIncoming)?.Total ?? 0;
                // This will be true in the case of entities that don't have a concept of incoming / outgoing (Ride)
                if (perDayOfWeekDto.FirstOrDefault().IncomingTotal == null &&
                    perDayOfWeekDto.FirstOrDefault().OutgoingTotal == null &&
                    perDayOfWeekDto.FirstOrDefault().Total > 0)
                {
                    baseByDayOfWeekEntities[index].Total = perDayOfWeekDto.FirstOrDefault().Total;
                }
            }

            return _mapper.Map<List<T>>(baseByDayOfWeekEntities);
        }

        public List<T> BuildByHour<T>(List<PerHourEntity> entities)
        {
            var baseByHourEntities = SqlServer.Entities.PerHourHelper.GenerateBasePerHourDtos();
            foreach (var perHourDto in entities.GroupBy(m => new { m.Day, m.Hour }))
            {
                var index = baseByHourEntities.FindIndex(d => d.Day == perHourDto.Key.Day && d.Hour == perHourDto.Key.Hour);
                baseByHourEntities[index].IncomingTotal = perHourDto.FirstOrDefault(i => i.IsIncoming)?.Total;
                baseByHourEntities[index].OutgoingTotal = perHourDto.FirstOrDefault(i => !i.IsIncoming)?.Total;
                baseByHourEntities[index].Total = perHourDto.Sum(g => g.Total);
            }

            return _mapper.Map<List<T>>(baseByHourEntities);
        }

        public List<ByPeriod> BuildByPeriod(List<DateTime> calendarDays, List<PerformanceEntity> entityPerformance, DateTime? previousPeriodEndDate)
        {
            var periodPerformance = new List<ByPeriod>();
            if (!previousPeriodEndDate.HasValue)
            {
                previousPeriodEndDate = DateTime.MinValue;
            }
  
            var priorPeriodDays = calendarDays.Where(d => d < previousPeriodEndDate).ToList();
            var currentPeriodDays = calendarDays.Where(d => d >= previousPeriodEndDate && d <= DateTime.Today).ToList();
            for (var i = 0; i < priorPeriodDays.Count(); i++)
            {
                var performanceEntity = entityPerformance.FirstOrDefault(m => m.OccurredDate == priorPeriodDays[i]);
                periodPerformance.Add(new ByPeriod()
                {
                    DayNumber = i + 1,
                    IsPreviousPeriod = true,
                    Count = performanceEntity?.Count ?? 0
                });
            }
            for (var i = 0; i < currentPeriodDays.Count(); i++)
            {
                var performanceEntity = entityPerformance.FirstOrDefault(m => m.OccurredDate == currentPeriodDays[i]);
                periodPerformance.Add(new ByPeriod()
                {
                    DayNumber = i + 1,
                    IsPreviousPeriod = false,
                    Count = performanceEntity?.Count ?? 0
                });
            }

            return periodPerformance;
        }
    }
}
