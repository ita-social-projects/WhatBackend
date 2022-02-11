using CharlieBackend.Business.Services.FileServices.ExportCSVFilesService;
using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Core.DTO.Export;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices
{
    public class CsvExportService : IExportService
    {
        IUnitOfWork _unitOfWork;

        public CsvExportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<FileDto>> GetListofStudentsByGroupId(long groupId)
        {
            var group = await _unitOfWork.StudentGroupRepository.GetByIdAsync(
                 groupId);

            if (group != null)
            {
                var students = await _unitOfWork.StudentRepository
                        .GetStudentsByIdGroups(group.Id);

                if (students.Count == 0)
                {
                    return Result<FileDto>.GetError(ErrorCode.NotFound,
                            "Group hasn't any students");
                }

                using var fileExporter = new CsvStudentsGroupExport(group.Name);

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
                        "Group not found");
            }
        }

        public Task<Result<FileDto>> GetStudentClassbook(StudentsClassbookResultDto data)
        {
            return Task.FromResult(Result<FileDto>.GetError(
                    ErrorCode.ValidationError,
                    "student classbook can't be returned in csv format"));
        }

        public Task<Result<FileDto>> GetStudentGroupResults(StudentGroupsResultsDto data)
        {
            return Task.FromResult(Result<FileDto>.GetError(
                   ErrorCode.ValidationError,
                   "student group results can't be returned in csv format"));
        }

        public Task<Result<FileDto>> GetStudentResults(StudentsResultsDto data)
        {
            return Task.FromResult(Result<FileDto>.GetError(
                   ErrorCode.ValidationError,
                   "student results can't be returned in csv format"));
        }

        public Task<Result<FileDto>> GetStudentsClassbook(StudentsClassbookResultDto data)
        {
            return Task.FromResult(Result<FileDto>.GetError(
                   ErrorCode.ValidationError,
                   "students classbook can't be returned in csv format"));
        }

        public Task<Result<FileDto>> GetStudentsResults(StudentsResultsDto data)
        {
            return Task.FromResult(Result<FileDto>.GetError(
                   ErrorCode.ValidationError,
                   "student results can't be returned in csv format"));
        }
    }
}
