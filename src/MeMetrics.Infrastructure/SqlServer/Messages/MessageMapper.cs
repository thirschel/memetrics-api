using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MeMetrics.Domain.Models.Messages;
using MeMetrics.Infrastructure.SqlServer.Entities;

namespace MeMetrics.Infrastructure.SqlServer.Messages
{
    public class MessageMapper : IMessageMapper
    {
        private readonly IMapper _mapper;

        public MessageMapper(
            IMapper mapper)
        {
            _mapper = mapper;
        }

        public int GetTotalMessagesByGender(List<MessageByGenderEntity> entities, string gender)
        {
            foreach (var messageByGenderDto in entities.GroupBy(m => m.Gender))
            {
                if (messageByGenderDto.Key == gender)
                {
                    return messageByGenderDto.Sum(m => m.Total);
                }
            }

            return 0;
        }

        public int GetAverageTextLengthByGender(List<MessageByGenderEntity> entities, string gender)
        {
            foreach (var messageByGenderDto in entities.GroupBy(m => m.Gender))
            {
                var outgoingTextLength = messageByGenderDto.Where(m => !m.IsIncoming);
                if (messageByGenderDto.Key == gender)
                {
                    return (int)outgoingTextLength.Average(m => m.AverageTextLength);
                }
            }

            return 0;
        }

        public List<MessageByDayOfWeek> BuildMessagesByDayOfWeek(List<PerDayOfWeekEntity> entities)
        {
            var baseByDayOfWeekEntities = DayOfTheWeekHelper.GenerateBaseDayOfWeekDtos();
            foreach (var perDayOfWeekDto in entities.GroupBy(m => m.DayOfWeek))
            {
                var index = baseByDayOfWeekEntities.FindIndex(d => d.DayOfWeek == perDayOfWeekDto.First().DayOfWeek);
                baseByDayOfWeekEntities[index].IncomingTotal = perDayOfWeekDto.FirstOrDefault(i => i.IsIncoming)?.Total;
                baseByDayOfWeekEntities[index].OutgoingTotal = perDayOfWeekDto.FirstOrDefault(i => !i.IsIncoming)?.Total;
            }

            return _mapper.Map<List<MessageByDayOfWeek>>(baseByDayOfWeekEntities);
        }

        public List<MessageByHour> BuildMessagesByHour(List<PerHourEntity> entities)
        {
            var baseByHourEntities = PerHourHelper.GenerateBasePerHourDtos();
            foreach (var perHourDto in entities.GroupBy(m => new { m.Day, m.Hour }))
            {
                var index = baseByHourEntities.FindIndex(d => d.Day == perHourDto.Key.Day && d.Hour == perHourDto.Key.Hour);
                baseByHourEntities[index].IncomingTotal = perHourDto.FirstOrDefault(i => i.IsIncoming)?.Total;
                baseByHourEntities[index].OutgoingTotal = perHourDto.FirstOrDefault(i => !i.IsIncoming)?.Total;
                baseByHourEntities[index].Total = perHourDto.Sum(g => g.Total);
            }

            return _mapper.Map<List<MessageByHour>>(baseByHourEntities);
        }

        public List<MessageByPeriod> BuildMessagesByPeriod(List<DateTime> calendarDays, List<PerformanceEntity> callsPerformance, DateTime previousPeriodEndDate)
        {
            var callPerformance = new List<MessageByPeriod>();
            if (callsPerformance.Any())
            {
                var priorPeriodDays = calendarDays.Where(d => d < previousPeriodEndDate).ToList();
                var currentPeriodDays = calendarDays.Where(d => d > previousPeriodEndDate && d <= DateTime.Today).ToList();
                for (var i = 0; i < priorPeriodDays.Count(); i++)
                {
                    var count = callsPerformance.FirstOrDefault(m => m.OccurredDate == priorPeriodDays[i]);
                    callPerformance.Add(new MessageByPeriod()
                    {
                        DayNumber = i + 1,
                        IsPreviousPeriod = true,
                        Count = count?.Count ?? 0
                    });
                }
                for (var i = 0; i < currentPeriodDays.Count(); i++)
                {
                    var count = callsPerformance.FirstOrDefault(m => m.OccurredDate == currentPeriodDays[i]);
                    callPerformance.Add(new MessageByPeriod()
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
