using CharlieBackend.Business.Helpers;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.Models.ResultModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ImportFileServices.Csv
{
    public class StudentsGroupCsvFileImporter
    {
        private readonly IStudentGroupService _studentGroupService;
        private readonly IAccountService _accountService;
        private readonly IStudentService _studentService;

        public StudentsGroupCsvFileImporter(
                IStudentGroupService studentGroupService,
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
            var accounts = await СollectAccountsFromFilePath(filePath);

            if (accounts.Error != null)
            {
                return Result<GroupWithStudentsDto>.GetError(
                        accounts.Error.Code,
                        accounts.Error.Message);
            }

            if (accounts.Data.Count < 1)
            {
                return Result<GroupWithStudentsDto>.GetError(
                        ErrorCode.ValidationError,
                        "file hasn't any student accounts");
            }

            var students = await CreateStudents(accounts.Data);

            if (students.Error != null)
            {
                return Result<GroupWithStudentsDto>.GetError(
                        students.Error.Code,
                        students.Error.Message);
            }

            studentGroup.StudentIds = students.Data.Select(s => s.Id).ToList();

            var group = await _studentGroupService.CreateStudentGroupAsync(
                    studentGroup);

            if (group.Error != null)
            {
                return Result<GroupWithStudentsDto>.GetError(group.Error.Code,
                        group.Error.Message);
            }

            var detailedGroup = new GroupWithStudentsDto
            {
                Id = group.Data.Id,
                CourseId = group.Data.CourseId,
                Name = group.Data.Name,
                StartDate = group.Data.StartDate,
                FinishDate = group.Data.FinishDate,
                Students = students.Data
            };

            return Result<GroupWithStudentsDto>.GetSuccess(detailedGroup);
        }

        private async Task<Result<IList<AccountDto>>> СollectAccountsFromFilePath(
                 string filePath)
        {
            var accountsForStudents = new List<AccountDto>();

            using StreamReader stream = new StreamReader(filePath);

            string[] studentsInfo = null;

            while (stream.Peek() != -1)
            {
                sbyte count = 0;
                var line = stream.ReadLine().Trim('\n');

                studentsInfo = line.Split(';');

                var account = new CreateAccountDto()
                {
                    FirstName = studentsInfo[count++],
                    LastName = studentsInfo[count++],
                    Email = studentsInfo[count++],
                };

                if (await _accountService.IsEmailTakenAsync(account.Email))
                {
                    var accountSearch = await _accountService
                            .GetAccountCredentialsByEmailAsync(account.Email);

                    accountsForStudents.Add(accountSearch.Data);
                }
                else
                {
                    var _tempPassword = PasswordHelper.GeneratePassword();

                    account.Password = _tempPassword;
                    account.ConfirmPassword = _tempPassword;

                    var accountCreating = await _accountService
                        .CreateAccountAsync(account);

                    if (accountCreating.Error != null)
                    {
                        return Result<IList<AccountDto>>.GetError(
                                accountCreating.Error.Code,
                                accountCreating.Error.Message);
                    }
                    else
                    {
                        accountsForStudents.Add(accountCreating.Data);
                    }
                }
            }

            return Result<IList<AccountDto>>.GetSuccess(accountsForStudents);
        }

        private async Task<Result<IList<StudentDto>>> CreateStudents(
                IList<AccountDto> accounts)
        {
            IList<StudentDto> students = new List<StudentDto>();

            foreach (var account in accounts)
            {
                var studentSearch = await _studentService
                        .GetStudentByEmailAsync(account.Email);

                if (studentSearch.Data != null)
                {
                    students.Add(studentSearch.Data);
                }
                else
                {
                    if (studentSearch.Error != null)
                    {
                        return Result<IList<StudentDto>>.GetError(
                                studentSearch.Error.Code,
                                studentSearch.Error.Message);
                    }

                    var student = await _studentService.CreateStudentAsync(
                           account.Id);

                    if (student.Error != null)
                    {
                        return Result<IList<StudentDto>>.GetError(
                                student.Error.Code,
                                student.Error.Message);
                    }
                    else
                    {
                        students.Add(student.Data);
                    }
                }
            }

            return Result<IList<StudentDto>>.GetSuccess(students);
        }
    }
}

