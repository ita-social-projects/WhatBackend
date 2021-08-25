using AutoMapper;
using CharlieBackend.Business.Helpers;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices
{
    public class StudentsGroupXlsxFileImporter : IStudentsGroupXlsxFileImporter
    {
        private readonly IBaseFileService _baseFileService;
        private readonly IMapper _mapper;
        private readonly IStudentGroupService _studentGroupService;
        private readonly IStudentService _studentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;

        public StudentsGroupXlsxFileImporter(
            IBaseFileService baseFileService,
            IMapper mapper,
            IStudentGroupService studentGroupService,
            IStudentService studentService,
            IAccountService accountService,
            IUnitOfWork unitOfWork)
        {
            _baseFileService = baseFileService;
            _mapper = mapper;
            _studentGroupService = studentGroupService;
            _studentService = studentService;
            _accountService = accountService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<GroupWithStudentsDto>> ImportGroupAsync(long courseId, IFormFile file)
        {
            #region pre_validation
            if (file == null)
            {
                return Result<GroupWithStudentsDto>.GetError(ErrorCode.ValidationError, "File was not provided");
            }

            if (!_baseFileService.IsFileExtensionValid(file))
            {
                return Result<GroupWithStudentsDto>.GetError(ErrorCode.ValidationError, "File extension not supported");
            }
            #endregion

            var filePath = await _baseFileService.UploadFileAsync(file);

            var students = new List<StudentDto>();

            var detailedGroup = new GroupWithStudentsDto();

            using (IXLWorkbook book = new XLWorkbook(filePath))
            {
                #region group_creation
                var group = await _studentGroupService.CreateStudentGroupAsync(new CreateStudentGroupDto
                {
                    CourseId = courseId,
                    Name = book.Worksheet(1).Row(2).Cell((int)StudentsGroupWorksheetHeader.GroupName).GetValue<string>(),
                    StartDate = Convert.ToDateTime(book.Worksheet(1).Row(2).Cell((int)StudentsGroupWorksheetHeader.StartDate).GetValue<string>()),
                    FinishDate = Convert.ToDateTime(book.Worksheet(1).Row(2).Cell((int)StudentsGroupWorksheetHeader.FinishDate).GetValue<string>()),
                });

                if (group.Error != null)
                {
                    return Result<GroupWithStudentsDto>.GetError(group.Error.Code, group.Error.Message);
                }

                detailedGroup = new GroupWithStudentsDto
                {
                    Id = group.Data.Id,
                    CourseId = group.Data.CourseId,
                    Name = group.Data.Name,
                    StartDate = group.Data.StartDate,
                    FinishDate = group.Data.FinishDate
                };
                #endregion

                foreach (IXLRow row in book.Worksheet(1).RowsUsed().Skip(1))
                {
                    #region account_creation
                    var _tempPassword = PasswordHelper.GeneratePassword();

                    var account = await _accountService.CreateAccountAsync(new CreateAccountDto
                    {
                        Email = row.Cell((int)StudentsGroupWorksheetHeader.StudentEmail).GetValue<string>(),
                        FirstName = row.Cell((int)StudentsGroupWorksheetHeader.StudentFirstName).GetValue<string>(),
                        LastName = row.Cell((int)StudentsGroupWorksheetHeader.StudentLastName).GetValue<string>(),
                        Password = _tempPassword,
                        ConfirmPassword = _tempPassword
                    });

                    if (account.Error != null)
                    {
                        return Result<GroupWithStudentsDto>.GetError(account.Error.Code, account.Error.Message);
                    }
                    #endregion

                    var student = await _studentService.CreateStudentAsync(account.Data.Id);

                    if (student.Error != null)
                    {
                        return Result<GroupWithStudentsDto>.GetError(student.Error.Code, student.Error.Message);
                    }

                    students.Add(student.Data);
                }

                detailedGroup.Students = students;
            }

            File.Delete(filePath);

            await _unitOfWork.CommitAsync();

            return Result<GroupWithStudentsDto>.GetSuccess(detailedGroup);
        }
    }
}
