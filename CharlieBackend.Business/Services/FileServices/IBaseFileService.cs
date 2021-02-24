using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices
{
    public interface IBaseFileService
    {
        bool IsFileExtensionValid(IFormFile file);

        Task<string> UploadFileAsync(IFormFile file);
    }
}
