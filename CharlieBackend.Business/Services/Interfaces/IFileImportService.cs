using ClosedXML.Excel;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.DTO.File;
using CharlieBackend.Core.FileModels;
using CharlieBackend.Core.Models.ResultModel;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IFileImportService
    {
        public Task<Result<List<StudentGroupFileModel>>> ImportFileAsync(ImportFileDto file);
    }
}
