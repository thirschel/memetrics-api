using System.Collections.Generic;
using System.Threading.Tasks;
using MeMetrics.Domain.Models.Attachments;

namespace MeMetrics.Application.Interfaces
{
    public interface IAttachmentRepository
    {
        Task InsertAttachments(IList<Attachment> attachments);
    }
}
