namespace MeMetrics.Domain.Models.RecruitmentMessage
{
    public class RecruitmentMessageByDayOfWeek : ByDayOfWeek
    {
        public int Incoming { get; set; }
        public int Outgoing { get; set; }
    }
}