using CharlieBackend.Core.FileModels;
using CharlieBackend.Core.Models.ResultModel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IStudentImportService
    {
        public Task<Result<List<StudentFileModel>>> ImportFileAsync(long groupId, IFormFile uploadedFile);

        public bool CheckIfExcelFile(IFormFile file);
    }
}