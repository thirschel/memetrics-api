using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Bogus;
using MeMetrics.Application.Models.Enums;
using MeMetrics.Domain.Models.Calls;
using MeMetrics.Domain.Models.Messages;
using MeMetrics.Domain.Models.RecruitmentMessage;
using MeMetrics.Domain.Models.Rides;
using MeMetrics.Infrastructure.Helpers;
using MeMetrics.Infrastructure.SqlServer.Entities;
using Xunit;

namespace MeMetrics.Infrastructure.Tests.SqlServer
{
    public class MapperHelperTests
    {

        [Fact]
        public void BuildByDayOfWeek_Should_Add_Missing_Days_In_Order()
        {
            var configuration = new MapperConfiguration(cfg => { cfg.AddProfile<PerDayOfWeekProfile>(); });
            var mapper = new Mapper(configuration);
            var mapperHelper = new EntityMapper(mapper);

            var perDayOfWeekFaker = new Faker<PerDayOfWeekEntity>()
                .RuleFor(p => p.Total, f => f.Random.Int(0, 10000))
                .RuleFor(p => p.DayOfWeek, f => f.PickRandom<DayOfWeekEnum>())
                .RuleFor(p => p.IsIncoming, f => f.Random.Bool())
                .RuleFor(p => p.OutgoingTotal, f => f.Random.Int(0, 10000))
                .RuleFor(p => p.IncomingTotal, f => f.Random.Int(0, 10000));


            var entity = perDayOfWeekFaker.Generate();

            var entities = new List<PerDayOfWeekEntity>() {
                entity,
            };

            var callsByDayOfWeek = mapperHelper.BuildByDayOfWeek<CallByDayOfWeek>(entities);

            Assert.Equal(7, callsByDayOfWeek.Count());
            Assert.Equal(Enum.GetName(typeof(DayOfWeekEnum), DayOfWeekEnum.Sunday), callsByDayOfWeek[0].DayOfWeek);
            Assert.Equal(Enum.GetName(typeof(DayOfWeekEnum), DayOfWeekEnum.Monday), callsByDayOfWeek[1].DayOfWeek);
            Assert.Equal(Enum.GetName(typeof(DayOfWeekEnum), DayOfWeekEnum.Tuesday), callsByDayOfWeek[2].DayOfWeek);
            Assert.Equal(Enum.GetName(typeof(DayOfWeekEnum), DayOfWeekEnum.Wednesday), callsByDayOfWeek[3].DayOfWeek);
            Assert.Equal(Enum.GetName(typeof(DayOfWeekEnum), DayOfWeekEnum.Thursday), callsByDayOfWeek[4].DayOfWeek);
            Assert.Equal(Enum.GetName(typeof(DayOfWeekEnum), DayOfWeekEnum.Friday), callsByDayOfWeek[5].DayOfWeek);
            Assert.Equal(Enum.GetName(typeof(DayOfWeekEnum), DayOfWeekEnum.Saturday), callsByDayOfWeek[6].DayOfWeek);
        }

        [Fact]
        public void BuildByDayOfWeek_Can_Map_To_Call()
        {
            var configuration = new MapperConfiguration(cfg => { cfg.AddProfile<PerDayOfWeekProfile>(); });
            var mapper = new Mapper(configuration);
            var mapperHelper = new EntityMapper(mapper);

            var perDayOfWeekFaker = new Faker<PerDayOfWeekEntity>()
                .RuleFor(p => p.Total, f => f.Random.Int(0, 10000))
                .RuleFor(p => p.DayOfWeek, f => f.PickRandom<DayOfWeekEnum>())
                .RuleFor(p => p.IsIncoming, f => f.Random.Bool())
                .RuleFor(p => p.OutgoingTotal, f => f.Random.Int(0, 10000))
                .RuleFor(p => p.IncomingTotal, f => f.Random.Int(0, 10000));


            // Should have 1 complete day which needs a IsIncoming = true and an IsIncoming = false
            var entity = perDayOfWeekFaker.Generate();
            var secondEntity = perDayOfWeekFaker.Generate();
            secondEntity.IsIncoming = !entity.IsIncoming;
            secondEntity.DayOfWeek = entity.DayOfWeek;

            var entities = new List<PerDayOfWeekEntity>() {
                entity,
                secondEntity
            };
            
            var callsByDayOfWeek = mapperHelper.BuildByDayOfWeek<CallByDayOfWeek>(entities);
            var callDayOfWeek = callsByDayOfWeek.First(c => c.DayOfWeek == Enum.GetName(typeof(DayOfWeekEnum), entity.DayOfWeek));

            var incomingTotal = entity.IsIncoming ? entity.Total : secondEntity.Total;
            var outgoingTotal = entity.IsIncoming ? secondEntity.Total : entity.Total;

            Assert.Equal(incomingTotal, callDayOfWeek.Incoming);
            Assert.Equal(outgoingTotal, callDayOfWeek.Outgoing);
            Assert.Equal(incomingTotal + outgoingTotal, callDayOfWeek.Total);
        }

        [Fact]
        public void BuildByDayOfWeek_Can_Map_To_Message()
        {
            var configuration = new MapperConfiguration(cfg => { cfg.AddProfile<PerDayOfWeekProfile>(); });
            var mapper = new Mapper(configuration);
            var mapperHelper = new EntityMapper(mapper);

            var perDayOfWeekFaker = new Faker<PerDayOfWeekEntity>()
                .RuleFor(p => p.Total, f => f.Random.Int(0, 10000))
                .RuleFor(p => p.DayOfWeek, f => f.PickRandom<DayOfWeekEnum>())
                .RuleFor(p => p.IsIncoming, f => f.Random.Bool())
                .RuleFor(p => p.OutgoingTotal, f => f.Random.Int(0, 10000))
                .RuleFor(p => p.IncomingTotal, f => f.Random.Int(0, 10000));


            // Should have 1 complete day which needs a IsIncoming = true and an IsIncoming = false
            var entity = perDayOfWeekFaker.Generate();
            var secondEntity = perDayOfWeekFaker.Generate();
            secondEntity.IsIncoming = !entity.IsIncoming;
            secondEntity.DayOfWeek = entity.DayOfWeek;

            var entities = new List<PerDayOfWeekEntity>() {
                entity,
                secondEntity
            };

            var messagesByDayOfWeek = mapperHelper.BuildByDayOfWeek<MessageByDayOfWeek>(entities);
            var messageDayOfWeek = messagesByDayOfWeek.First(c => c.DayOfWeek == Enum.GetName(typeof(DayOfWeekEnum), entity.DayOfWeek));

            var incomingTotal = entity.IsIncoming ? entity.Total : secondEntity.Total;
            var outgoingTotal = entity.IsIncoming ? secondEntity.Total : entity.Total;

            Assert.Equal(incomingTotal, messageDayOfWeek.Incoming);
            Assert.Equal(outgoingTotal, messageDayOfWeek.Outgoing);
            Assert.Equal(incomingTotal + outgoingTotal, messageDayOfWeek.Total);
        }

        [Fact]
        public void BuildByDayOfWeek_Can_Map_To_Ride()
        {
            var configuration = new MapperConfiguration(cfg => { cfg.AddProfile<PerDayOfWeekProfile>(); });
            var mapper = new Mapper(configuration);
            var mapperHelper = new EntityMapper(mapper);

            var perDayOfWeekFaker = new Faker<PerDayOfWeekEntity>()
                .RuleFor(p => p.Total, f => f.Random.Int(0, 10000))
                .RuleFor(p => p.DayOfWeek, f => f.PickRandom<DayOfWeekEnum>())
                .RuleFor(p => p.IsIncoming, f => f.Random.Bool())
                .RuleFor(p => p.OutgoingTotal, f => null)
                .RuleFor(p => p.IncomingTotal, f => null);


            var entity = perDayOfWeekFaker.Generate();

            var entities = new List<PerDayOfWeekEntity>() {
                entity,
            };

            var ridesByDayOfWeek = mapperHelper.BuildByDayOfWeek<RideByDayOfWeek>(entities);
            var rideDayOfWeek = ridesByDayOfWeek.First(c => c.DayOfWeek == Enum.GetName(typeof(DayOfWeekEnum), entity.DayOfWeek));

            Assert.Equal(entity.Total, rideDayOfWeek.Total);
        }

        [Fact]
        public void BuildByDayOfWeek_Can_Map_To_RecruitmentMessage()
        {
            var configuration = new MapperConfiguration(cfg => { cfg.AddProfile<PerDayOfWeekProfile>(); });
            var mapper = new Mapper(configuration);
            var mapperHelper = new EntityMapper(mapper);

            var perDayOfWeekFaker = new Faker<PerDayOfWeekEntity>()
                .RuleFor(p => p.Total, f => f.Random.Int(0, 10000))
                .RuleFor(p => p.DayOfWeek, f => f.PickRandom<DayOfWeekEnum>())
                .RuleFor(p => p.IsIncoming, f => f.Random.Bool())
                .RuleFor(p => p.OutgoingTotal, f => f.Random.Int(0, 10000))
                .RuleFor(p => p.IncomingTotal, f => f.Random.Int(0, 10000));


            // Should have 1 complete day which needs a IsIncoming = true and an IsIncoming = false
            var entity = perDayOfWeekFaker.Generate();
            var secondEntity = perDayOfWeekFaker.Generate();
            secondEntity.IsIncoming = !entity.IsIncoming;
            secondEntity.DayOfWeek = entity.DayOfWeek;

            var entities = new List<PerDayOfWeekEntity>() {
                entity,
                secondEntity
            };

            var recruitmentMessagesByDayOfWeek = mapperHelper.BuildByDayOfWeek<RecruitmentMessageByDayOfWeek>(entities);
            var recruitmentMessageDayOfWeek = recruitmentMessagesByDayOfWeek.First(c => c.DayOfWeek == Enum.GetName(typeof(DayOfWeekEnum), entity.DayOfWeek));

            var incomingTotal = entity.IsIncoming ? entity.Total : secondEntity.Total;
            var outgoingTotal = entity.IsIncoming ? secondEntity.Total : entity.Total;

            Assert.Equal(incomingTotal, recruitmentMessageDayOfWeek.Incoming);
            Assert.Equal(outgoingTotal, recruitmentMessageDayOfWeek.Outgoing);
            Assert.Equal(incomingTotal + outgoingTotal, recruitmentMessageDayOfWeek.Total);
        }

        [Fact]
        public void BuildByHour_Should_Add_Missing_Days_In_Order()
        {
            var configuration = new MapperConfiguration(cfg => { cfg.AddProfile<PerHourProfile>(); });
            var mapper = new Mapper(configuration);
            var mapperHelper = new EntityMapper(mapper);

            var perDayOfWeekFaker = new Faker<PerHourEntity>()
                .RuleFor(p => p.Total, f => f.Random.Int(0, 10000))
                .RuleFor(p => p.Day, f => f.Random.Int(1,7))
                .RuleFor(p => p.Hour, f => f.Random.Int(0,23))
                .RuleFor(p => p.IsIncoming, f => f.Random.Bool())
                .RuleFor(p => p.OutgoingTotal, f => f.Random.Int(0, 10000))
                .RuleFor(p => p.IncomingTotal, f => f.Random.Int(0, 10000));


            var entity = perDayOfWeekFaker.Generate();

            var entities = new List<PerHourEntity>() {
                entity,
            };

            var callsByHour = mapperHelper.BuildByHour<CallByHour>(entities);

            Assert.Equal(168, callsByHour.Count());
            for (var day = 1; day < 8; day++)
            {
                for (var hour = 0; hour < 24; hour++)
                {
                    Assert.NotNull(callsByHour.Find(c => c.Day == day && c.Hour == hour));
                }
            }
        }

        [Fact]
        public void BuildByHour_Should_Map_To_Call()
        {
            var configuration = new MapperConfiguration(cfg => { cfg.AddProfile<PerHourProfile>(); });
            var mapper = new Mapper(configuration);
            var mapperHelper = new EntityMapper(mapper);

            var perHourFaker = new Faker<PerHourEntity>()
                .RuleFor(p => p.Total, f => f.Random.Int(0, 10000))
                .RuleFor(p => p.Day, f => f.Random.Int(1, 7))
                .RuleFor(p => p.Hour, f => f.Random.Int(0, 23))
                .RuleFor(p => p.IsIncoming, f => f.Random.Bool())
                .RuleFor(p => p.OutgoingTotal, f => f.Random.Int(0, 10000))
                .RuleFor(p => p.IncomingTotal, f => f.Random.Int(0, 10000));


            // Should have 1 complete day which needs a IsIncoming = true and an IsIncoming = false
            var entity = perHourFaker.Generate();
            var secondEntity = perHourFaker.Generate();
            secondEntity.IsIncoming = !entity.IsIncoming;
            secondEntity.Day = entity.Day;
            secondEntity.Hour = entity.Hour;

            var entities = new List<PerHourEntity>() {
                entity,
                secondEntity
            };

            var callsByHour = mapperHelper.BuildByHour<CallByHour>(entities);
            var callByHour = callsByHour.First(c => c.Day == entity.Day && c.Hour == entity.Hour);

            var incomingTotal = entity.IsIncoming ? entity.Total : secondEntity.Total;
            var outgoingTotal = entity.IsIncoming ? secondEntity.Total : entity.Total;

            Assert.Equal(incomingTotal, callByHour.Incoming);
            Assert.Equal(outgoingTotal, callByHour.Outgoing);
            Assert.Equal(incomingTotal + outgoingTotal, callByHour.Total);
        }

        [Fact]
        public void BuildByHour_Should_Map_To_Message()
        {
            var configuration = new MapperConfiguration(cfg => { cfg.AddProfile<PerHourProfile>(); });
            var mapper = new Mapper(configuration);
            var mapperHelper = new EntityMapper(mapper);

            var perHourFaker = new Faker<PerHourEntity>()
                .RuleFor(p => p.Total, f => f.Random.Int(0, 10000))
                .RuleFor(p => p.Day, f => f.Random.Int(1, 7))
                .RuleFor(p => p.Hour, f => f.Random.Int(0, 23))
                .RuleFor(p => p.IsIncoming, f => f.Random.Bool())
                .RuleFor(p => p.OutgoingTotal, f => f.Random.Int(0, 10000))
                .RuleFor(p => p.IncomingTotal, f => f.Random.Int(0, 10000));


            // Should have 1 complete day which needs a IsIncoming = true and an IsIncoming = false
            var entity = perHourFaker.Generate();
            var secondEntity = perHourFaker.Generate();
            secondEntity.IsIncoming = !entity.IsIncoming;
            secondEntity.Day = entity.Day;
            secondEntity.Hour = entity.Hour;

            var entities = new List<PerHourEntity>() {
                entity,
                secondEntity
            };

            var messagesByHour = mapperHelper.BuildByHour<MessageByHour>(entities);
            var messageByHour = messagesByHour.First(c => c.Day == entity.Day && c.Hour == entity.Hour);

            var incomingTotal = entity.IsIncoming ? entity.Total : secondEntity.Total;
            var outgoingTotal = entity.IsIncoming ? secondEntity.Total : entity.Total;

            Assert.Equal(incomingTotal, messageByHour.Incoming);
            Assert.Equal(outgoingTotal, messageByHour.Outgoing);
            Assert.Equal(incomingTotal + outgoingTotal, messageByHour.Total);
        }

        [Fact]
        public void BuildByHour_Should_Map_To_Ride()
        {
            var configuration = new MapperConfiguration(cfg => { cfg.AddProfile<PerHourProfile>(); });
            var mapper = new Mapper(configuration);
            var mapperHelper = new EntityMapper(mapper);

            var perHourFaker = new Faker<PerHourEntity>()
                .RuleFor(p => p.Total, f => f.Random.Int(0, 10000))
                .RuleFor(p => p.Day, f => f.Random.Int(1, 7))
                .RuleFor(p => p.Hour, f => f.Random.Int(0, 23))
                .RuleFor(p => p.IsIncoming, f => f.Random.Bool())
                .RuleFor(p => p.OutgoingTotal, f => f.Random.Int(0, 10000))
                .RuleFor(p => p.IncomingTotal, f => f.Random.Int(0, 10000));


            // Should have 1 complete day which needs a IsIncoming = true and an IsIncoming = false
            var entity = perHourFaker.Generate();
            var secondEntity = perHourFaker.Generate();
            secondEntity.IsIncoming = !entity.IsIncoming;
            secondEntity.Day = entity.Day;
            secondEntity.Hour = entity.Hour;

            var entities = new List<PerHourEntity>() {
                entity,
                secondEntity
            };

            var ridesByHour = mapperHelper.BuildByHour<CallByHour>(entities);
            var rideByHour = ridesByHour.First(c => c.Day == entity.Day && c.Hour == entity.Hour);

            var incomingTotal = entity.IsIncoming ? entity.Total : secondEntity.Total;
            var outgoingTotal = entity.IsIncoming ? secondEntity.Total : entity.Total;

            Assert.Equal(incomingTotal, rideByHour.Incoming);
            Assert.Equal(outgoingTotal, rideByHour.Outgoing);
            Assert.Equal(incomingTotal + outgoingTotal, rideByHour.Total);
        }

        [Fact]
        public void BuildByHour_Should_Map_To_RecruitmentMessage()
        {
            var configuration = new MapperConfiguration(cfg => { cfg.AddProfile<PerHourProfile>(); });
            var mapper = new Mapper(configuration);
            var mapperHelper = new EntityMapper(mapper);

            var perHourFaker = new Faker<PerHourEntity>()
                .RuleFor(p => p.Total, f => f.Random.Int(0, 10000))
                .RuleFor(p => p.Day, f => f.Random.Int(1, 7))
                .RuleFor(p => p.Hour, f => f.Random.Int(0, 23))
                .RuleFor(p => p.IsIncoming, f => f.Random.Bool())
                .RuleFor(p => p.OutgoingTotal, f => f.Random.Int(0, 10000))
                .RuleFor(p => p.IncomingTotal, f => f.Random.Int(0, 10000));


            // Should have 1 complete day which needs a IsIncoming = true and an IsIncoming = false
            var entity = perHourFaker.Generate();
            var secondEntity = perHourFaker.Generate();
            secondEntity.IsIncoming = !entity.IsIncoming;
            secondEntity.Day = entity.Day;
            secondEntity.Hour = entity.Hour;

            var entities = new List<PerHourEntity>() {
                entity,
                secondEntity
            };

            var recruitmentMessagesByHour = mapperHelper.BuildByHour<CallByHour>(entities);
            var recruitmentMessageByHour = recruitmentMessagesByHour.First(c => c.Day == entity.Day && c.Hour == entity.Hour);

            var incomingTotal = entity.IsIncoming ? entity.Total : secondEntity.Total;
            var outgoingTotal = entity.IsIncoming ? secondEntity.Total : entity.Total;

            Assert.Equal(incomingTotal, recruitmentMessageByHour.Incoming);
            Assert.Equal(outgoingTotal, recruitmentMessageByHour.Outgoing);
            Assert.Equal(incomingTotal + outgoingTotal, recruitmentMessageByHour.Total);
        }

        [Fact]
        public void BuildByPeriod_Should_Map_Correctly()
        {
            var configuration = new MapperConfiguration(cfg => { cfg.AddProfile<PerPeriodProfile>(); });
            var mapper = new Mapper(configuration);
            var mapperHelper = new EntityMapper(mapper);

            var performanceFaker = new Faker<PerformanceEntity>()
                .RuleFor(p => p.Count, f => f.Random.Int(0, 10000));

            var entities = new List<PerformanceEntity>();
            var calendarDays = new List<DateTime>();

            for (var i = -10; i < 0; i++)
            {
                var occurredDate = new DateTime(DateTime.Now.AddDays(i).Year, DateTime.Now.AddDays(i).Month, DateTime.Now.AddDays(i).Day);
                var entity = performanceFaker.Generate();
                entity.OccurredDate = occurredDate;
                entities.Add(entity);
                calendarDays.Add(occurredDate);
            }

            var byPeriod = mapperHelper.BuildByPeriod(calendarDays, entities, new DateTime(DateTime.Now.AddDays(-5).Year, DateTime.Now.AddDays(-5).Month, DateTime.Now.AddDays(-5).Day));

            Assert.Equal(5, byPeriod.Count(c => c.IsPreviousPeriod));
            Assert.Equal(5, byPeriod.Count(c => !c.IsPreviousPeriod));
            Assert.Equal(entities[0].Count, byPeriod[0].Count);
            Assert.Equal(1, byPeriod[0].DayNumber);
        }
    }
}
