using AutoMapper;
using CharlieBackend.Business.Services.Interfaces;
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
    public class GroupFileImporter : IGroupFileImporter
    {
        private readonly IBaseFileService _baseFileService;
        private readonly IMapper _mapper;
        private readonly IStudentGroupService _studentGroupService;
        private readonly IUnitOfWork _unitOfWork;

        public GroupFileImporter(IBaseFileService baseFileService, IMapper mapper, IStudentGroupService studentGroupService, IUnitOfWork unitOfWork)
        {
            _baseFileService = baseFileService;
            _mapper = mapper;
            _studentGroupService = studentGroupService;
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

            var filePath = await _baseFileService.UploadFileAsync(file);

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
    }
}
