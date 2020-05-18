using System;
using AutoMapper;
using MeMetrics.Application.Models.Enums;
using MeMetrics.Domain.Models.Calls;
using MeMetrics.Domain.Models.Messages;
using MeMetrics.Domain.Models.RecruitmentMessage;
using MeMetrics.Domain.Models.Rides;

namespace MeMetrics.Infrastructure.SqlServer.Entities
{
    public class PerDayOfWeekProfile : Profile
    {
        public PerDayOfWeekProfile()
        {
            CreateMap<PerDayOfWeekEntity, MessageByDayOfWeek>()
                .ForMember(dest => dest.Total, source => source.MapFrom(s => s.IncomingTotal + s.OutgoingTotal))
                .ForMember(dest => dest.Incoming, source => source.MapFrom(s => s.IncomingTotal))
                .ForMember(dest => dest.Outgoing, source => source.MapFrom(s => s.OutgoingTotal))
                .ForMember(dest => dest.DayOfWeek, source => source.MapFrom(s => Enum.GetName(typeof(DayOfWeekEnum), s.DayOfWeek)))
                .ReverseMap();
            CreateMap<PerDayOfWeekEntity, CallByDayOfWeek>()
                .ForMember(dest => dest.Total, source => source.MapFrom(s => s.IncomingTotal + s.OutgoingTotal))
                .ForMember(dest => dest.Incoming, source => source.MapFrom(s => s.IncomingTotal))
                .ForMember(dest => dest.Outgoing, source => source.MapFrom(s => s.OutgoingTotal))
                .ForMember(dest => dest.DayOfWeek, source => source.MapFrom(s => Enum.GetName(typeof(DayOfWeekEnum), s.DayOfWeek)))
                .ReverseMap();
            CreateMap<PerDayOfWeekEntity, RideByDayOfWeek>()
                .ForMember(dest => dest.Total, source => source.MapFrom(s => s.Total))
                .ForMember(dest => dest.DayOfWeek, source => source.MapFrom(s => Enum.GetName(typeof(DayOfWeekEnum), s.DayOfWeek)))
                .ReverseMap();
            CreateMap<PerDayOfWeekEntity, RecruitmentMessageByDayOfWeek>()
                .ForMember(dest => dest.Total, source => source.MapFrom(s => s.IncomingTotal + s.OutgoingTotal))
                .ForMember(dest => dest.Incoming, source => source.MapFrom(s => s.IncomingTotal))
                .ForMember(dest => dest.Outgoing, source => source.MapFrom(s => s.OutgoingTotal))
                .ForMember(dest => dest.DayOfWeek, source => source.MapFrom(s => Enum.GetName(typeof(DayOfWeekEnum), s.DayOfWeek)))
                .ReverseMap();
        }
    }
}
