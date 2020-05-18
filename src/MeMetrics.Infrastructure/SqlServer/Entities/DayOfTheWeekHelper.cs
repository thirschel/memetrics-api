using System.Collections.Generic;
using MeMetrics.Application.Models.Enums;

namespace MeMetrics.Infrastructure.SqlServer.Entities
{
    public class DayOfTheWeekHelper
    {
        public static List<PerDayOfWeekEntity> GenerateBaseDayOfWeekDtos()
        {
            return new List<PerDayOfWeekEntity>()
            {
                new PerDayOfWeekEntity(){DayOfWeek = DayOfWeekEnum.Sunday},
                new PerDayOfWeekEntity(){DayOfWeek = DayOfWeekEnum.Monday},
                new PerDayOfWeekEntity(){DayOfWeek = DayOfWeekEnum.Tuesday},
                new PerDayOfWeekEntity(){DayOfWeek = DayOfWeekEnum.Wednesday},
                new PerDayOfWeekEntity(){DayOfWeek = DayOfWeekEnum.Thursday},
                new PerDayOfWeekEntity(){DayOfWeek = DayOfWeekEnum.Friday},
                new PerDayOfWeekEntity(){DayOfWeek = DayOfWeekEnum.Saturday},
            };
        }
    }
}
