using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Core.Models.ResultModel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices
{
    public interface IXLSFileService : IGroupFileImporter, IStudentFileImporter, IThemeFileImporter
    {
        //Task<Result<IEnumerable<ImportStudentGroupDto>>> ImportGroupsAsync(long coursId, IFormFile file);

        //Task<Result<IEnumerable<StudentDto>>> ImportStudentsAsync(long groupId, IFormFile file);

        //Task<Result<IEnumerable<ThemeDto>>> ImportThemesAsync(IFormFile file);
    }
}
