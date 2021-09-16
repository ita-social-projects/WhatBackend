using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Core.DTO.Export;
using CharlieBackend.Core.Models.ResultModel;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices
{
    public class ExportServiceXlsx : IExportService
    {
        public ExportServiceXlsx()
        {
        }

        public async Task<Result<FileDto>> GetStudentsClassbook(StudentsClassbookResultDto data)
        {
            using var classbook = new ClassbookExportXlsx();

            await classbook.FillFile(data);
            classbook.AdjustContent();

            return Result<FileDto>.GetSuccess(new FileDto
            {
                ByteArray = await classbook.GetByteArrayAsync(),
                ContentType = classbook.GetContentType(),
                Filename = classbook.GetFileName()
            });
        }

        public async Task<Result<FileDto>> GetStudentsResults(StudentsResultsDto data)
        {
            using var result = new StudentsResultsExportXlsx();

            await result.FillFile(data);
            result.AdjustContent();

            return Result<FileDto>.GetSuccess(new FileDto
            {
                ByteArray = await result.GetByteArrayAsync(),
                ContentType = result.GetContentType(),
                Filename = result.GetFileName()
            });
        }

        public async Task<Result<FileDto>> GetStudentClassbook(StudentsClassbookResultDto data)
        {
            using var result = new StudentClassbookXlsx();

            await result.FillFile(data);
            result.AdjustContent();

            return Result<FileDto>.GetSuccess(new FileDto
            {
                ByteArray = await result.GetByteArrayAsync(),
                ContentType = result.GetContentType(),
                Filename = result.GetFileName()
            });
        }

        public async Task<Result<FileDto>> GetStudentResults(StudentsResultsDto data)
        {
            using var result = new StudentResultXlsx();

            await result.FillFile(data);
            result.AdjustContent();

            return Result<FileDto>.GetSuccess(new FileDto
            {
                ByteArray = await result.GetByteArrayAsync(),
                ContentType = result.GetContentType(),
                Filename = result.GetFileName()
            });
        }

        public async Task<Result<FileDto>> GetStudentGroupResults(StudentGroupsResultsDto data)
        {
            using var result = new StudentGroupResultsXlsx();

            await result.FillFile(data);
            result.AdjustContent();

            return Result<FileDto>.GetSuccess(new FileDto
            {
                ByteArray = await result.GetByteArrayAsync(),
                ContentType = result.GetContentType(),
                Filename = result.GetFileName()
            });
        }

        public Task<Result<FileDto>> GetListofStudentsByGroupId(long groupId)
        {
            return Task.FromResult(Result<FileDto>.GetError(
                    ErrorCode.ValidationError,
                    "List of student group can't be returned in xlsx file format"));
        }
    }
}

