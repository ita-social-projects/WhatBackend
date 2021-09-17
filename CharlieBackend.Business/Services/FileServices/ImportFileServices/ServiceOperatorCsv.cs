using CharlieBackend.Business.Services.FileServices.ImportFileServices.Csv;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ImportFileServices
{
    public class ServiceOperatorCsv : IOperatorImport
    {
        private readonly IStudentGroupService _studentGroupService;
        private readonly IAccountService _accountService;
        private readonly IStudentService _studentService;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceOperatorCsv(IStudentGroupService studentGroupService,
            IAccountService accountService,
            IStudentService studentService,
            IUnitOfWork unitOfWork)
        {
            _studentGroupService = studentGroupService;
            _accountService = accountService;
            _studentService = studentService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<GroupWithStudentsDto>> ImportGroupAsync(
                CreateStudentGroupDto studentGroup, string filePath)
        {
            var groupCreator = new StudentsGroupCsvFileImporter(
                    _studentGroupService, _accountService, _studentService);

            return await groupCreator.ImportGroupAsync(studentGroup, filePath);
        }

        public Task<Result<IEnumerable<ThemeDto>>> ImportThemesAsync(
                string path)
        {
            return Task.FromResult(Result<IEnumerable<ThemeDto>>.GetError(
                    ErrorCode.NotFound,
                    "Application can't do import themes from csv yet"));
        }
    }
}
