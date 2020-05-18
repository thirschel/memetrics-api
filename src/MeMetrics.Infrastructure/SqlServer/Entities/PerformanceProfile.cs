using AutoMapper;
using MeMetrics.Domain.Models.Calls;
using MeMetrics.Domain.Models.Messages;
using MeMetrics.Domain.Models.RecruitmentMessage;
using MeMetrics.Domain.Models.Rides;

namespace MeMetrics.Infrastructure.SqlServer.Entities
{
    public class PerPeriodProfile : Profile
    {
        public PerPeriodProfile() {
            CreateMap<MessageByPeriod, PerformanceEntity>()
                .ForMember(dest => dest.IsPreviousPeriod, source => source.MapFrom(s => s.Count))
                .ForMember(dest => dest.Count, source => source.MapFrom(s => s.Count))
                .ReverseMap();
            CreateMap<RideByPeriod, PerformanceEntity>()
                .ForMember(dest => dest.Count, source => source.MapFrom(s => s.Count))
                .ReverseMap();
            CreateMap<CallByPeriod, PerformanceEntity>()
                .ForMember(dest => dest.Count, source => source.MapFrom(s => s.Count))
                .ReverseMap();
            CreateMap<RecruitmentMessageByPeriod, PerformanceEntity>()
                .ForMember(dest => dest.Count, source => source.MapFrom(s => s.Count))
                .ReverseMap();
        }
    }
}
