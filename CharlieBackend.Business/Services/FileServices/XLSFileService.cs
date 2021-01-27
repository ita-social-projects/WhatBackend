using AutoMapper;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Core.Entities;
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
    public class XLSFileService : IXLSFileService
    {
        private readonly string _tempPassword = "changeYourPassword";
        private readonly IAccountService _accountService;
        private readonly IBaseFileService _baseFileService;
        private readonly IMapper _mapper;
        private readonly IStudentGroupService _studentGroupService;
        private readonly IStudentService _studentService;
        private readonly IThemeService _themeService;
        private readonly IUnitOfWork _unitOfWork;

        public XLSFileService(IAccountService accountService,
                              IBaseFileService baseFileService,
                              IMapper mapper,
                              IStudentGroupService studentGroupService,
                              IStudentService studentService,
                              IThemeService themeService,
                              IUnitOfWork unitOfWork)
        {
            _accountService = accountService;
            _baseFileService = baseFileService;
            _mapper = mapper;
            _studentGroupService = studentGroupService;
            _studentService = studentService;
            _themeService = themeService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<ImportStudentGroupDto>>> ImportGroupsAsync(long coursId, IFormFile file)
        {
            if (file == null)
            {
                return Result<IEnumerable<ImportStudentGroupDto>>.GetError(ErrorCode.ValidationError, "File was not provided");
            }

            if (!_baseFileService.IsFileExtensionValid(file))
            {
                return Result<IEnumerable<ImportStudentGroupDto>>.GetError(ErrorCode.ValidationError, "File extension not supported");
            }

            var filePath = await _baseFileService.UploadFile(file);

            var groups = new List<ImportStudentGroupDto>();

            using (IXLWorkbook book = new XLWorkbook(filePath))
            {
                foreach (IXLRow row in book.Worksheet(1).RowsUsed().Skip(1))
                {
                    var group = await _studentGroupService.CreateStudentGroupAsync(new CreateStudentGroupDto
                    {
                        CourseId = coursId,
                        Name = row.Cell((int)GroupsWorksheetHeader.GroupName).GetValue<string>(),
                        StartDate = Convert.ToDateTime(row.Cell((int)GroupsWorksheetHeader.StartDate).GetValue<string>()),
                        FinishDate = Convert.ToDateTime(row.Cell((int)GroupsWorksheetHeader.FinishDate).GetValue<string>()),
                    });

                    if (group.Error != null)
                    {
                        return Result<IEnumerable<ImportStudentGroupDto>>.GetError(group.Error.Code, group.Error.Message);
                    }

                    groups.Add(_mapper.Map<ImportStudentGroupDto>(group.Data));
                }
            }

            File.Delete(filePath);

            await _unitOfWork.CommitAsync();

            return Result<IEnumerable<ImportStudentGroupDto>>.GetSuccess(groups);
        }

        public async Task<Result<IEnumerable<StudentDto>>> ImportStudentsAsync(long groupId, IFormFile uploadedFile)
        {
            if (uploadedFile == null)
            {
                return Result<IEnumerable<StudentDto>>.GetError(ErrorCode.ValidationError, "File was not provided");
            }

            if (!_baseFileService.IsFileExtensionValid(uploadedFile))
            {
                return Result<IEnumerable<StudentDto>>.GetError(ErrorCode.ValidationError, "File extension not supported");
            }

            var groupExist = await _unitOfWork.StudentGroupRepository.IsEntityExistAsync(groupId);

            if (!groupExist)
            {
                return Result<IEnumerable<StudentDto>>.GetError(ErrorCode.NotFound, $"Group with id: {groupId} does not exist");
            }

            var filePath = await _baseFileService.UploadFile(uploadedFile);

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

        public async Task<Result<IEnumerable<ThemeDto>>> ImportThemesAsync(IFormFile file)
        {
            if (file == null)
            {
                return Result<IEnumerable<ThemeDto>>.GetError(ErrorCode.ValidationError, "File was not provided");
            }

            if (!_baseFileService.IsFileExtensionValid(file))
            {
                return Result<IEnumerable<ThemeDto>>.GetError(ErrorCode.ValidationError, "File extension not supported");
            }

            var filePath = await _baseFileService.UploadFile(file);

            var themes = new List<ThemeDto>();

            using (IXLWorkbook book = new XLWorkbook(filePath))
            {
                foreach (IXLRow row in book.Worksheet(1).RowsUsed().Skip(1))
                {
                    var theme = await _themeService.CreateThemeAsync(new CreateThemeDto
                    {
                        Name = row.Cell(1).GetValue<string>(),
                    });

                    if (theme.Error != null)
                    {
                        return Result<IEnumerable<ThemeDto>>.GetError(theme.Error.Code, theme.Error.Message);
                    }

                    themes.Add(theme.Data);
                }
            }

            File.Delete(filePath);

            await _unitOfWork.CommitAsync();

            return Result<IEnumerable<ThemeDto>>.GetSuccess(themes);
        }
    }
}
