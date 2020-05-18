using System.Collections.Generic;

namespace MeMetrics.Infrastructure.SqlServer.Entities
{
    public class PerHourHelper
    {
        public static List<PerHourEntity> GenerateBasePerHourDtos()
        {
            var baseEntities = new List<PerHourEntity>();
            for(var day = 1; day < 8; day++)
            {
                for (var hour = 0; hour < 24; hour++)
                {
                    baseEntities.Add(new PerHourEntity(){Day = day, Hour = hour });
                }
            }

            return baseEntities;
        }
    }
}
