using System.Threading;
using MeMetrics.Application.Models;
using MeMetrics.Application.Interfaces;
using Moq;
using Serilog;
using Xunit;
using MeMetrics.Application.Commands.RecruitmentMessage;
using MeMetrics.Domain.Models.RecruitmentMessage;

namespace MeMetrics.Application.Tests.Commands.Example
{
    public class CreateRecruitmentMessageCommandHandlerTests
    {
        [Fact]
        public async void Request_With_No_Id_Should_Return_Invalid_Input()
        {
           // ARRANGE
           var validator = new CreateRecruitmentMessageCommandValidator();
           var recruitmentMessageRepositoryMock = new Mock<IRecruitmentMessageRepository>();
           var loggerMock = new Mock<ILogger>();

           var handler = new CreateRecruitmentMessageCommandHandler(
               recruitmentMessageRepositoryMock.Object,
               loggerMock.Object,
               validator
           );

           // ACT
           var response = await handler.Handle(new CreateRecruitmentMessageCommand() { RecruitmentMessage = new RecruitmentMessage() }, new CancellationToken());

           // ASSERT
           Assert.Equal(CommandResultTypeEnum.InvalidInput, response.Type);
        }

        [Fact]
        public async void Should_RecruitmentMessage_InsertRecruitmentMessage_In_Repository()
        {
            // ARRANGE
            var validator = new CreateRecruitmentMessageCommandValidator();
            var recruitmentMessageRepositoryMock = new Mock<IRecruitmentMessageRepository>();
            var loggerMock = new Mock<ILogger>();

            var handler = new CreateRecruitmentMessageCommandHandler(
                recruitmentMessageRepositoryMock.Object,
                loggerMock.Object,
                validator
            );

            recruitmentMessageRepositoryMock.Setup(x => x.InsertRecruitmentMessage(It.IsAny<RecruitmentMessage>())).ReturnsAsync(1);

            // ACT
            var response = await handler.Handle(new CreateRecruitmentMessageCommand(){RecruitmentMessage = new RecruitmentMessage() { RecruiterId = "1" }}, new CancellationToken());

           // ASSERT
           Assert.Equal(CommandResultTypeEnum.Success, response.Type);
            recruitmentMessageRepositoryMock.Verify(x => x.InsertRecruitmentMessage(It.IsAny<RecruitmentMessage>()), Times.Once);
        }

        [Fact]
        public async void Should_Return_Unprocessable_If_No_Rows_Affected()
        {
            // ARRANGE
            var validator = new CreateRecruitmentMessageCommandValidator();
            var recruitmentMessageRepositoryMock = new Mock<IRecruitmentMessageRepository>();
            var loggerMock = new Mock<ILogger>();

            var handler = new CreateRecruitmentMessageCommandHandler(
                recruitmentMessageRepositoryMock.Object,
                loggerMock.Object,
                validator
            );

            recruitmentMessageRepositoryMock.Setup(x => x.InsertRecruitmentMessage(It.IsAny<RecruitmentMessage>())).ReturnsAsync(0);

            // ACT
            var response = await handler.Handle(new CreateRecruitmentMessageCommand() { RecruitmentMessage = new RecruitmentMessage() { RecruiterId = "1" } }, new CancellationToken());

            // ASSERT
            Assert.Equal(CommandResultTypeEnum.UnprocessableEntity, response.Type);
            recruitmentMessageRepositoryMock.Verify(x => x.InsertRecruitmentMessage(It.IsAny<RecruitmentMessage>()), Times.Once);
        }
    }
}
