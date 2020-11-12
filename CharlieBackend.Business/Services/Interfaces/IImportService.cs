using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.DTO.File;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Core.FileModels;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IImportService
    {
        public Task<Result<List<StudentGroupFileModel>>> ImportFileAsync(ImportFileDto file);
    }
}
