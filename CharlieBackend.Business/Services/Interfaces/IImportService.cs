using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.DTO.File;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.ResultModel;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IImportService
    {
        public Task<Result<List<StudentGroup>>> ImportFileAsync(ImportFileDto file);
    }
}
