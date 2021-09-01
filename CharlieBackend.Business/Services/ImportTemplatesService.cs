using CharlieBackend.Business.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class ImportTemplatesService : IImportTemplatesService
    {
        private readonly string _basePath = "FileTemplates";

        public string GetContentType()
        {
            return "application/octet-stream";
        }

        public string GetFullFilePath(string fileName)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), _basePath, fileName);
        }

        public async Task<byte[]> GetByteArrayAsync(string fileName)
        {
            return await Task.Run(() => GetByteArray(fileName));
        }

        private byte[] GetByteArray(string fileName)
        {
            return File.ReadAllBytes(GetFullFilePath(fileName));
        }

        public string GetGroupTemplateFileName()
        {
            return "ImportGroup.xlsx";
        }

        public string GetThemeTemplateFileName()
        {
            return "ImportThemes.xlsx";
        }
    }
}
