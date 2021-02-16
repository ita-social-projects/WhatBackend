using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices
{
    public class StudentXlsFileImporter : IStudentXlsFileImporter
    {
        private readonly string _tempPassword = "changeYourPassword";

        private readonly IAccountService _accountService;
        private readonly IBaseFileService _baseFileService;
        private readonly IStudentGroupService _studentGroupService;
        private readonly IStudentService _studentService;
        private readonly IUnitOfWork _unitOfWork;

        public StudentXlsFileImporter(IAccountService accountService,
                                   IBaseFileService baseFileService,
                                   IStudentGroupService studentGroupService,
                                   IStudentService studentService,
                                   IUnitOfWork unitOfWork)
        {
            _accountService = accountService;
            _baseFileService = baseFileService;
            _studentGroupService = studentGroupService;
            _studentService = studentService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<StudentDto>>> ImportStudentsAsync(long groupId, IFormFile file)
        {
            if (file == null)
            {
                return Result<IEnumerable<StudentDto>>.GetError(ErrorCode.ValidationError, "File was not provided");
            }

            if (!_baseFileService.IsFileExtensionValid(file))
            {
                return Result<IEnumerable<StudentDto>>.GetError(ErrorCode.ValidationError, "File extension not supported");
            }

            var groupExist = await _unitOfWork.StudentGroupRepository.IsEntityExistAsync(groupId);

            if (!groupExist)
            {
                return Result<IEnumerable<StudentDto>>.GetError(ErrorCode.NotFound, $"Group with id: {groupId} does not exist");
            }

            var filePath = await _baseFileService.UploadFileAsync(file);

            var students = new List<StudentDto>();

            using (IXLWorkbook book = new XLWorkbook(filePath))
            {
                foreach (IXLRow row in book.Worksheet(1).RowsUsed().Skip(1))
                {
                    var account = await _accountService.CreateAccountAsync(new CreateAccountDto
                    {
                        Email = row.Cell((int)StudentsWorksheetHeader.Email).GetValue<string>(),
                        FirstName = row.Cell((int)StudentsWorksheetHeader.FirstName).GetValue<string>(),
                        LastName = row.Cell((int)StudentsWorksheetHeader.LastName).GetValue<string>(),
                        Password = _tempPassword,
                        ConfirmPassword = _tempPassword
                    });

                    if (account.Error != null)
                    {
                        return Result<IEnumerable<StudentDto>>.GetError(account.Error.Code, account.Error.Message);
                    }

                    var student = await _studentService.CreateStudentAsync(account.Data.Id);

                    if (student.Error != null)
                    {
                        return Result<IEnumerable<StudentDto>>.GetError(student.Error.Code, student.Error.Message);
                    }

                    students.Add(student.Data);
                }
            }

            _studentGroupService.AddStudentOfStudentGroups(students.Select(x => new StudentOfStudentGroup
            {
                StudentGroupId = groupId,
                StudentId = x.Id
            }));

            File.Delete(filePath);

            await _unitOfWork.CommitAsync();

            return Result<IEnumerable<StudentDto>>.GetSuccess(students);
        }
    }
}
