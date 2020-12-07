using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using CharlieBackend.Core.FileModels;
using CharlieBackend.Core.Models.ResultModel;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IStudentImportService
    {
        Task<Result<List<StudentFileModel>>> ImportFileAsync(long groupId, IFormFile uploadedFile);

        bool CheckIfExcelFile(IFormFile file);
    }
}
