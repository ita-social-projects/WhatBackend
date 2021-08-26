using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.Models.ResultModel;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices
{
    public interface IStudentsGroupXlsxFileImporter
    {
        Task<Result<GroupWithStudentsDto>> ImportGroupAsync(long courseId, IFormFile file);
    }
}
