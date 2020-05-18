using System.Threading.Tasks;
using MeMetrics.Domain.Models.Attachments;

namespace MeMetrics.Application.Interfaces
{
    public interface IAttachmentRepository
    {
        Task InsertAttachment(Attachment attachment, string messageId1);
    }
}