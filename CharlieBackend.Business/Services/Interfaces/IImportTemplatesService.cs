using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IImportTemplatesService
    {
        string GetContentType();

        string GetFullFilePath(string fileName);

        Task<byte[]> GetByteArrayAsync(string fileName);

        string GetGroupTemplateFileName();

        string GetThemeTemplateFileName();
    }
}
