using CharlieBackend.Business.Services.FileServices.ExportFileServices.Html;
using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Core.DTO.Export;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Threading.Tasks;


namespace CharlieBackend.Business.Services.FileServices.ExportFileServices
{
    public class ExportServiceHtml : IExportService
    {
        IUnitOfWork _unitOfWork;
        public ExportServiceHtml(IUnitOfWork unitOfWork)
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
                            "Group hasn't any students");
                }

                using var fileExporter = new StudentGroupsExportHTML(group.Name);

                fileExporter.FillFile(students);

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

        public async Task<Result<FileDto>> GetStudentClassbook(StudentsClassbookResultDto data)
        {
            return Result<FileDto>.GetError(
                    ErrorCode.ValidationError,
                    "student classbook can't be returned in html format");
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
            using var classbook = new ClassbookExportHTML();

            classbook.FillFile(data);

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
