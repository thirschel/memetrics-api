using AutoMapper;
using MeMetrics.Domain.Models.Calls;
using MeMetrics.Domain.Models.Messages;
using MeMetrics.Domain.Models.Rides;

namespace MeMetrics.Infrastructure.SqlServer.Entities
{
    public class PerHourProfile : Profile
    {
        public PerHourProfile()
        {
            CreateMap<MessageByHour, PerHourEntity>()
                .ForMember(dest => dest.Total, source => source.MapFrom(s => s.Total))
                .ForMember(dest => dest.IncomingTotal, source => source.MapFrom(s => s.Incoming))
                .ForMember(dest => dest.OutgoingTotal, source => source.MapFrom(s => s.Outgoing))
                .ReverseMap();
            CreateMap<CallByHour, PerHourEntity>()
                .ForMember(dest => dest.Total, source => source.MapFrom(s => s.Total))
                .ForMember(dest => dest.IncomingTotal, source => source.MapFrom(s => s.Incoming))
                .ForMember(dest => dest.OutgoingTotal, source => source.MapFrom(s => s.Outgoing))
                .ReverseMap();
            CreateMap<RideByHour, PerHourEntity>()
                .ForMember(dest => dest.Total, source => source.MapFrom(s => s.Total))
                .ReverseMap();

        }
    }
}
