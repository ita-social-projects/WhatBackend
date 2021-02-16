using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices
{
    public class BaseFileService : IBaseFileService
    {
        private readonly string _basePath = @"Upload\Files";

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            string generatedFileName = DateTime.Now.Ticks + Path.GetExtension(file.FileName);

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), _basePath);

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            string fullFilePath = Path.Combine(Directory.GetCurrentDirectory(), _basePath, generatedFileName);

            using (var stream = new FileStream(fullFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fullFilePath;
        }

        public bool IsFileExtensionValid(IFormFile file)
        {
            return Enum.TryParse(value: Path.GetExtension(file.FileName)[1..],
                                 ignoreCase: true,
                                 result: out FileExtension _);
        }
    }
}
