using System.Collections.Generic;
using System.Threading;
using MeMetrics.Application.Commands.RecruitmentMessage;
using MeMetrics.Application.Interfaces;
using MeMetrics.Application.Models;
using Moq;
using Serilog;
using Xunit;

namespace MeMetrics.Application.Tests.Commands.RecruitmentMessage
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
           var response = await handler.Handle(new CreateRecruitmentMessageCommand() { RecruitmentMessages = new List<Domain.Models.RecruitmentMessage.RecruitmentMessage>() { new Domain.Models.RecruitmentMessage.RecruitmentMessage() }}, new CancellationToken());

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

            recruitmentMessageRepositoryMock.Setup(x => x.InsertRecruitmentMessages(It.IsAny<List<Domain.Models.RecruitmentMessage.RecruitmentMessage>>())).ReturnsAsync(1);

            // ACT
            var response = await handler.Handle(new CreateRecruitmentMessageCommand(){RecruitmentMessages = new List<Domain.Models.RecruitmentMessage.RecruitmentMessage>() {new Domain.Models.RecruitmentMessage.RecruitmentMessage() { RecruiterId = "1" }}}, new CancellationToken());

           // ASSERT
           Assert.Equal(CommandResultTypeEnum.Success, response.Type);
            recruitmentMessageRepositoryMock.Verify(x => x.InsertRecruitmentMessages(It.IsAny<List<Domain.Models.RecruitmentMessage.RecruitmentMessage>>()), Times.Once);
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

            recruitmentMessageRepositoryMock.Setup(x => x.InsertRecruitmentMessages(It.IsAny<List<Domain.Models.RecruitmentMessage.RecruitmentMessage>>())).ReturnsAsync(0);

            // ACT
            var response = await handler.Handle(new CreateRecruitmentMessageCommand() { RecruitmentMessages = new List<Domain.Models.RecruitmentMessage.RecruitmentMessage>() {new Domain.Models.RecruitmentMessage.RecruitmentMessage() { RecruiterId = "1" }} }, new CancellationToken());

            // ASSERT
            Assert.Equal(CommandResultTypeEnum.UnprocessableEntity, response.Type);
            recruitmentMessageRepositoryMock.Verify(x => x.InsertRecruitmentMessages(It.IsAny<List<Domain.Models.RecruitmentMessage.RecruitmentMessage>>()), Times.Once);
        }
    }
}
