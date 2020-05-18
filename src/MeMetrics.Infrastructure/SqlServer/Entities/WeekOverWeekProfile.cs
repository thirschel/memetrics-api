using AutoMapper;
using MeMetrics.Domain.Models;

namespace MeMetrics.Infrastructure.SqlServer.Entities
{
    public class WeekOverWeekProfile : Profile
    {
        public WeekOverWeekProfile()
        {
            CreateMap<ByWeek, WeekOverWeekEntity>()
                .ForMember(dest => dest.Total, source => source.MapFrom(s=> s.Total))
                .ReverseMap();
        }
    }
}
