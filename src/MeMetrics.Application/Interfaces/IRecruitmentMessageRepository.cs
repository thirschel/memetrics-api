using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MeMetrics.Domain.Models.RecruitmentMessage;

namespace MeMetrics.Application.Interfaces
{
    public interface IRecruitmentMessageRepository
    {
        Task<List<RecruitmentMessage>> GetRecruitmentMessages();
        Task<int> InsertRecruitmentMessage(RecruitmentMessage recruitmentMessage);

        Task<RecruitmentMessageMetrics> GetOverviewRecruitmentMessageMetrics(DateTime? startDate, DateTime endDate, DateTime? previousPeriodStartDate, DateTime? previousPeriodEndDate);
    }
}
