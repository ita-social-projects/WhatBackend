using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ImportFileServices
{
    public class ServiceOperatorXlsx : IOperatorImport
    {
        private readonly IStudentGroupService _studentGroupService;
        private readonly IStudentService _studentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;
        private readonly IThemeService _themeService;

        public ServiceOperatorXlsx(IStudentGroupService studentGroupService,
                IStudentService studentService,
                IUnitOfWork unitOfWork,
                IAccountService accountService,
                IThemeService themeService)
        {
            _studentGroupService = studentGroupService;
            _studentService = studentService;
            _unitOfWork = unitOfWork;
            _accountService = accountService;
            _themeService = themeService;
        }

        public Task<Result<GroupWithStudentsDto>> ImportGroupAsync(
                CreateStudentGroupDto studentGroup, string filePath)
        {
            var groupCreator = new StudentsGroupXlsxFileImporter(
                    _studentGroupService, _studentService,
                    _accountService, _unitOfWork);

            return groupCreator.ImportGroupAsync(studentGroup, filePath);
        }

        public Task<Result<IEnumerable<ThemeDto>>> ImportThemesAsync(
                string filePath)
        {
            var themeCreator = new ThemeXlsFileImporter(_themeService,
                    _unitOfWork);

            return themeCreator.ImportThemesAsync(filePath);
        }
    }
}
