using CharlieBackend.Core.DTO.Export;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices
{
    public interface IFileService : IDisposable
    {
        bool IsFileExtensionValid(IFormFile file, out FileExtension extension);

        Task<string> UploadFileAsync(IFormFile file);
    }
}
