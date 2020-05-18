using System;

namespace MeMetrics.Infrastructure.SqlServer.Sql
{
    public class GetCalendarDatesInRange
    {
        public static string Value(DateTime? previousPeriodStartDate)
        {
            var startDateVariable = previousPeriodStartDate.HasValue ? "@previousPeriodStartDate" : "@startDate";
            return $@"SELECT Date
                    FROM Auxiliary.Calendar
                    WHERE Date >= {startDateVariable} AND Date < @endDate
                    ORDER BY Date ASC;";
        }
    }
}
