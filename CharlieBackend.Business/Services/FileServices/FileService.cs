using CharlieBackend.Core.DTO.Export;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices
{
    public class FileService : IFileService
    {
        private const string BASE_PATH = @"Upload\Files";

        public string CurrentFilePath { get; private set; }

        public FileService()
        {
            CurrentFilePath = string.Empty;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            string generatedFileName = DateTime.Now.Ticks + Path.GetExtension(file.FileName);

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), BASE_PATH);

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            CurrentFilePath = Path.Combine(Directory.GetCurrentDirectory(), BASE_PATH, generatedFileName);

            using (var stream = new FileStream(CurrentFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return CurrentFilePath;
        }

        public bool IsFileExtensionValid(IFormFile file, out FileExtension extension)
        {
            return Enum.TryParse(value: Path.GetExtension(file.FileName)[1..],
                                 ignoreCase: true,
                                 result: out extension);
        }

        public void Dispose()
        {
            if (File.Exists(CurrentFilePath))
            {
                File.Delete(CurrentFilePath);
            }
        }
    }
}
