using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Core.Models.ResultModel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices
{
    public class XLSFileService : IXLSFileService
    {
        private readonly IGroupFileImporter _groupFileImporter;
        private readonly IStudentFileImporter _studentFileImporter;
        private readonly IThemeFileImporter _themeFileImporter;

        public XLSFileService(IGroupFileImporter groupFileImporter,
                              IStudentFileImporter studentFileImporter,
                              IThemeFileImporter themeFileImporter)
        {
            _groupFileImporter = groupFileImporter;
            _studentFileImporter = studentFileImporter;
            _themeFileImporter = themeFileImporter;
        }

        public async Task<Result<IEnumerable<ImportStudentGroupDto>>> ImportGroupsAsync(long coursId, IFormFile file)
        {
            return await _groupFileImporter.ImportGroupsAsync(coursId, file);
        }

        public async Task<Result<IEnumerable<StudentDto>>> ImportStudentsAsync(long groupId, IFormFile file)
        {
            return await _studentFileImporter.ImportStudentsAsync(groupId, file);
        }

        public async Task<Result<IEnumerable<ThemeDto>>> ImportThemesAsync(IFormFile file)
        {
            return await _themeFileImporter.ImportThemesAsync(file);
        }
    }
}
