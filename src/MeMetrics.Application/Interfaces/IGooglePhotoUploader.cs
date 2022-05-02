using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MeMetrics.Application.Interfaces
{
    public interface IGooglePhotoUploader
    {
        Task<bool> UploadMedia(IFormFile file);
    }
}
