using CharlieBackend.Business.Services.FileServices.ImportFileServices.Xlsx;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Core.Models.ResultModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ImportFileServices.ImportOperators
{
    public class ServiceOperatorXlsx : IOperatorImport
    {
        private readonly IStudentGroupService _studentGroupService;
        private readonly IStudentService _studentService;
        private readonly IAccountService _accountService;
        private readonly IThemeService _themeService;

        public ServiceOperatorXlsx(IStudentGroupService studentGroupService,
                IStudentService studentService,
                IAccountService accountService,
                IThemeService themeService)
        {
            _studentGroupService = studentGroupService;
            _studentService = studentService;
            _accountService = accountService;
            _themeService = themeService;
        }

        public Task<Result<GroupWithStudentsDto>> ImportGroupAsync(
                CreateStudentGroupDto studentGroup, string filePath)
        {
            var groupCreator = new StudentsGroupXlsxFileImporter(
                    _studentGroupService, _studentService, _accountService);

            return groupCreator.ImportGroupAsync(studentGroup, filePath);
        }

        public Task<Result<IEnumerable<ThemeDto>>> ImportThemesAsync(
                string filePath)
        {
            var themeCreator = new ThemeXlsxFileImporter(_themeService);

            return themeCreator.ImportThemesAsync(filePath);
        }
    }
}
