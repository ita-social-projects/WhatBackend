using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.Models.ResultModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ImportFileServices.Base
{
    abstract public class StudentGroupFileImporter
    {
        protected readonly IStudentService _studentService;
        private readonly IStudentGroupService _studentGroupService;

        public StudentGroupFileImporter(IStudentService studentService,
                IStudentGroupService studentGroupService)
        {
            _studentService = studentService;
            _studentGroupService = studentGroupService;
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

        protected abstract Task<Result<IList<AccountDto>>> СollectAccountsFromFilePath(string filePath);

        protected async Task<Result<IList<StudentDto>>> CreateStudents(
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
