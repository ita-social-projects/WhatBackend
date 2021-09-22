using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Core.Models.ResultModel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ImportFileServices
{
    public interface IServiseImport
    {
        Task<Result<GroupWithStudentsDto>> ImportGroupAsync(IFormFile file, CreateStudentGroupDto group);

        Task<Result<IEnumerable<ThemeDto>>> ImportThemesAsync(IFormFile file);
    }
}
