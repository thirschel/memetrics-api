using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MeMetrics.Domain.Models.Calls;

namespace MeMetrics.Application.Interfaces
{
    public interface ICallRepository
    {
        Task<List<Call>> GetCalls();
        Task<int> InsertCalls(List<Call> calls);
        Task<CallMetrics> GetOverviewCallMetrics(DateTime? startDate, DateTime endDate, DateTime? previousPeriodStartDate, DateTime? previousPeriodEndDate);
    }
}
