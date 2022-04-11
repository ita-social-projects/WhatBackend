﻿using CharlieBackend.Business.Helpers;
using CharlieBackend.Business.Services.FileServices.ExportFileServices.Html;
using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Core.DTO.Export;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices
{
    public class HtmlExportService : IExportService
    {
        IUnitOfWork _unitOfWork;

        public HtmlExportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<FileDto>> GetListofStudentsByGroupId(long groupId)
        {
            var group = await _unitOfWork.StudentGroupRepository.GetByIdAsync(groupId);

            if (group != null)
            {
                var students = await _unitOfWork.StudentRepository
                        .GetStudentsByIdGroups(group.Id);

                if (students.Count == 0)
                {
                    return Result<FileDto>.GetError(ErrorCode.NotFound,
                            ResponseMessages.GroupHasNotStudents);
                }

                using var fileExporter = new StudentGroupsHtmlFileExport(group.Name);

                await fileExporter.FillFileAsync(students);

                return Result<FileDto>.GetSuccess(new FileDto()
                {
                    ByteArray = await fileExporter.GetByteArrayAsync(),
                    ContentType = fileExporter.GetContentType(),
                    Filename = fileExporter.GetFileName()
                });
            }
            else
            {
                return Result<FileDto>.GetError(ErrorCode.NotFound,
                        ResponseMessages.GroupNotFound);
            }
        }

        public async Task<Result<FileDto>> GetStudentClassbook(StudentsClassbookResultDto data)
        {
            using var classbook = new StudentClassbookHtmlFileExport();

            await classbook.FillFileAsync(data);

            return Result<FileDto>.GetSuccess(new FileDto()
            {
                ByteArray = await classbook.GetByteArrayAsync(),
                ContentType = classbook.GetContentType(),
                Filename = classbook.GetFileName()
            });
        }

        public Task<Result<FileDto>> GetStudentGroupResults(StudentGroupsResultsDto data)
        {
            return Task.FromResult(Result<FileDto>.GetError(
                    ErrorCode.ValidationError,
                    "student classbook can't be returned in html format"));
        }

        public Task<Result<FileDto>> GetStudentResults(StudentsResultsDto data)
        {
            return Task.FromResult(Result<FileDto>.GetError(
                    ErrorCode.ValidationError,
                    "student classbook can't be returned in html format"));
        }

        public async Task<Result<FileDto>> GetStudentsClassbook(StudentsClassbookResultDto data)
        {
            using var classbook = new ClassbookHtmlFileExport();

            await classbook.FillFileAsync(data);

            return Result<FileDto>.GetSuccess(new FileDto()
            {
                ByteArray = await classbook.GetByteArrayAsync(),
                ContentType = classbook.GetContentType(),
                Filename = classbook.GetFileName()
            });
        }

        public Task<Result<FileDto>> GetStudentsResults(StudentsResultsDto data)
        {
            return Task.FromResult(Result<FileDto>.GetError(
                    ErrorCode.ValidationError,
                    "student classbook can't be returned in html format"));
        }
    }
}