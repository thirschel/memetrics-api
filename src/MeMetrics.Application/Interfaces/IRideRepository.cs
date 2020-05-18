using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MeMetrics.Domain.Models.Rides;

namespace MeMetrics.Application.Interfaces
{
    public interface IRideRepository
    {
        Task<int> InsertRide(Ride ride);
        Task<RideMetrics> GetOverviewRideMetrics(DateTime? startDate, DateTime endDate, DateTime? previousPeriodStartDate, DateTime? previousPeriodEndDate);
    }
}
