using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.Models.ResultModel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices
{
    public interface IStudentFileImporter
    {
        Task<Result<IEnumerable<StudentDto>>> ImportStudentsAsync(long groupId, IFormFile file);
    }
}
