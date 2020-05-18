using System.Collections.Generic;
using MeMetrics.Infrastructure.SqlServer.Entities;

namespace MeMetrics.Infrastructure.Helpers
{
    public class PerHourHelper
    {
        public static List<PerHourEntity> GenerateBasePerHourDtos()
        {
            var baseDtos = new List<PerHourEntity>();
            for(var d = 1; d < 8; d++)
            {
                for (var h = 0; h < 24; h++)
                {
                    baseDtos.Add(new PerHourEntity(){Day = d, Hour = h});
                }
            }

            return baseDtos;
        }
    }
}
