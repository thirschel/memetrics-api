using System.Collections.Generic;
using System.Threading;
using MeMetrics.Application.Commands.Message;
using MeMetrics.Application.Interfaces;
using MeMetrics.Application.Models;
using MeMetrics.Domain.Models.Attachments;
using Moq;
using Serilog;
using Xunit;

namespace MeMetrics.Application.Tests.Commands.Message
{
    public class CreateMessageCommandHandlerTests
    {
        [Fact]
        public async void Handle_ShouldReturnInvalidRequest_WhenRequestHasNoMessageId()
        {
           // ARRANGE
           var validator = new CreateMessageCommandValidator();
           var messageRepositoryMock = new Mock<IMessageRepository>();
           var attachmentRepositoryMock = new Mock<IAttachmentRepository>();
           var loggerMock = new Mock<ILogger>();

           var handler = new CreateMessageCommandHandler(
               messageRepositoryMock.Object,
               attachmentRepositoryMock.Object,
               loggerMock.Object,
               validator
           );

           // ACT
           var response = await handler.Handle(new CreateMessagesCommand(){ Messages = new List<Domain.Models.Messages.Message>() { new Domain.Models.Messages.Message() } }, new CancellationToken());

           // ASSERT
           Assert.Equal(CommandResultTypeEnum.InvalidInput, response.Type);
        }

        [Fact]
        public async void Handle_ShouldCallInsertMessage()
        {
            // ARRANGE
            var validator = new CreateMessageCommandValidator();
            var messageRepositoryMock = new Mock<IMessageRepository>();
            var attachmentRepositoryMock = new Mock<IAttachmentRepository>();
            var loggerMock = new Mock<ILogger>();

            var handler = new CreateMessageCommandHandler(
                messageRepositoryMock.Object,
                attachmentRepositoryMock.Object,
                loggerMock.Object,
                validator
            );

            messageRepositoryMock.Setup(x => x.InsertMessages(It.IsAny<IList<Domain.Models.Messages.Message>>())).ReturnsAsync(1);

            // ACT
            var response = await handler.Handle(new CreateMessagesCommand(){Messages = new List<Domain.Models.Messages.Message>() { new() { MessageId = "1" } } }, new CancellationToken());

            // ASSERT
            Assert.Equal(CommandResultTypeEnum.Success, response.Type);
            messageRepositoryMock.Verify(x => x.InsertMessages(It.IsAny<IList<Domain.Models.Messages.Message>>()), Times.Once);
            attachmentRepositoryMock.Verify(x => x.InsertAttachments(It.IsAny<IList<Attachment>>()), Times.Never);
        }

        [Fact]
        public async void Handle_ShouldCallInsertAttachment_WhenRequestHasAttachments()
        {
            // ARRANGE
            var validator = new CreateMessageCommandValidator();
            var messageRepositoryMock = new Mock<IMessageRepository>();
            var attachmentRepositoryMock = new Mock<IAttachmentRepository>();
            var loggerMock = new Mock<ILogger>();

            var handler = new CreateMessageCommandHandler(
                messageRepositoryMock.Object,
                attachmentRepositoryMock.Object,
                loggerMock.Object,
                validator
            );
            var command = new CreateMessagesCommand()
            {
                Messages = new List<Domain.Models.Messages.Message>()
                {
                    new Domain.Models.Messages.Message()
                    {
                        MessageId = "1",
                        Attachments = new List<Attachment>()
                        {
                            new Attachment(),
                            new Attachment(),
                        }
                    }
                }
            };
            messageRepositoryMock.Setup(x => x.InsertMessages(It.IsAny<IList<Domain.Models.Messages.Message>>())).ReturnsAsync(1);

            // ACT
            var response = await handler.Handle(command, new CancellationToken());

            // ASSERT
            Assert.Equal(CommandResultTypeEnum.Success, response.Type);
            messageRepositoryMock.Verify(x => x.InsertMessages(It.IsAny<IList<Domain.Models.Messages.Message>>()), Times.Once);
            attachmentRepositoryMock.Verify(x => x.InsertAttachments(It.IsAny<IList<Attachment>>()), Times.Once);
        }

        [Fact]
        public async void Handle_ShouldReturnUnprocessable_WhenNoRowsAffected()
        {
            // ARRANGE
            var validator = new CreateMessageCommandValidator();
            var messageRepositoryMock = new Mock<IMessageRepository>();
            var attachmentRepositoryMock = new Mock<IAttachmentRepository>();
            var loggerMock = new Mock<ILogger>();

            var handler = new CreateMessageCommandHandler(
                messageRepositoryMock.Object,
                attachmentRepositoryMock.Object,
                loggerMock.Object,
                validator
            );

            messageRepositoryMock.Setup(x => x.InsertMessages(It.IsAny<IList<Domain.Models.Messages.Message>>())).ReturnsAsync(0);

            // ACT
            var response = await handler.Handle(new CreateMessagesCommand() { Messages = new List<Domain.Models.Messages.Message>() { new Domain.Models.Messages.Message() { MessageId = "1" } } }, new CancellationToken());

            // ASSERT
            Assert.Equal(CommandResultTypeEnum.UnprocessableEntity, response.Type);
            messageRepositoryMock.Verify(x => x.InsertMessages(It.IsAny< IList<Domain.Models.Messages.Message>>()), Times.Once);
        }
    }
}
