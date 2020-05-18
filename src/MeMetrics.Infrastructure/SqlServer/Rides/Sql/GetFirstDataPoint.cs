using System;

namespace MeMetrics.Infrastructure.SqlServer.Rides.Sql
{
    public class GetFirstDataPoint
    {
        public const string Value = @"
                    SELECT TOP 1 DATEADD(d, DATEDIFF(d, 0, RequestDate), 0) FROM [dbo].[Ride] ORDER BY RequestDate ASC;";
    }
}

