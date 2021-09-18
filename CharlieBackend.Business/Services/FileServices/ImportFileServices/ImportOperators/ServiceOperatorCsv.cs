using CharlieBackend.Business.Services.FileServices.ImportFileServices.Csv;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Core.Models.ResultModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ImportFileServices.ImportOperators
{
    public class ServiceOperatorCsv : IOperatorImport
    {
        private readonly IStudentGroupService _studentGroupService;
        private readonly IAccountService _accountService;
        private readonly IStudentService _studentService;

        public ServiceOperatorCsv(IStudentGroupService studentGroupService,
            IAccountService accountService,
            IStudentService studentService)
        {
            _studentGroupService = studentGroupService;
            _accountService = accountService;
            _studentService = studentService;
        }

        public async Task<Result<GroupWithStudentsDto>> ImportGroupAsync(
                CreateStudentGroupDto studentGroup, string filePath)
        {
            var groupCreator = new StudentsGroupCsvFileImporter(
                     _accountService, _studentGroupService, _studentService);

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
