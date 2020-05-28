using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MeMetrics.Application.Models;
using Moq;
using Serilog;
using Xunit;
using MeMetrics.Api.Controllers;
using MeMetrics.Application.Commands.RecruitmentMessage;
using MeMetrics.Application.Models.Enums;
using MeMetrics.Domain.Models.RecruitmentMessage;
using MeMetrics.Application.Queries.RecruitmentMessages;

namespace MeMetrics.Api.Tests
{
    public class RecruitmentMessageControllerTests
    {
        [Fact]
        public async void SaveRecruitmentMessage_Should_Return_Ok_Result()
        {
           var mediatorMock = new Mock<IMediator>();
           var loggerMock = new Mock<ILogger>();

           mediatorMock
               .Setup(x => x.Send(It.IsAny<CreateRecruitmentMessageCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new CommandResult<bool>(){ Type = CommandResultTypeEnum.Success });
           var controller = new RecruitmentMessageController(
               loggerMock.Object,
               mediatorMock.Object
           );

           var response = await controller.SaveRecruitmentMessage(new RecruitmentMessage());

           Assert.IsType<OkObjectResult>(response.Result);
           mediatorMock.Verify(x => x.Send(It.IsAny<CreateRecruitmentMessageCommand>(), It.IsAny<CancellationToken>()), Times.Once());

        }

        [Fact]
        public async void SaveRecruitmentMessage_Should_Return_Bad_Result_If_Invalid_Input()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger>();

            mediatorMock
                .Setup(x => x.Send(It.IsAny<CreateRecruitmentMessageCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CommandResult<bool>() { Type = CommandResultTypeEnum.InvalidInput });
            var controller = new RecruitmentMessageController(
                loggerMock.Object,
                mediatorMock.Object
            );

            var response = await controller.SaveRecruitmentMessage(new RecruitmentMessage());

            Assert.IsType<BadRequestResult>(response.Result);
        }

        [Fact]
        public async void GetRecruiterMetrics_Should_Return_Ok_Result()
        {

           var mediatorMock = new Mock<IMediator>();
           var loggerMock = new Mock<ILogger>();

           mediatorMock
               .Setup(x => x.Send(It.IsAny<GetRecruitmentMessageMetricsByPeriodSelectionQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new QueryResult<RecruitmentMessageMetrics>() { Type = QueryResultTypeEnum.Success });
           var controller = new RecruitmentMessageController(
               loggerMock.Object,
               mediatorMock.Object
           );

           var response = await controller.GetRecruiterMetrics(PeriodSelectionEnum.AllTime);

           Assert.IsType<OkObjectResult>(response.Result);
           mediatorMock.Verify(x => x.Send(It.IsAny<GetRecruitmentMessageMetricsByPeriodSelectionQuery>(), It.IsAny<CancellationToken>()), Times.Once());

        }

        [Fact]
        public async void GetRecruiterMetrics_Should_Return_Not_Found_Result_If_Data_Not_Found()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger>();

            mediatorMock
                .Setup(x => x.Send(It.IsAny<GetRecruitmentMessageMetricsByPeriodSelectionQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new QueryResult<RecruitmentMessageMetrics>() { Type = QueryResultTypeEnum.NotFound });
            var controller = new RecruitmentMessageController(
                loggerMock.Object,
                mediatorMock.Object
            );

            var response = await controller.GetRecruiterMetrics(PeriodSelectionEnum.AllTime);

            Assert.IsType<NotFoundResult>(response.Result);
        }
    }
}
