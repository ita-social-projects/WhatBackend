using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Core.DTO.Export;
using CharlieBackend.Core.Models.ResultModel;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices
{
    public class XlsxExportService : IExportService
    {
        public XlsxExportService()
        {
        }

        public async Task<Result<FileDto>> GetStudentsClassbook(StudentsClassbookResultDto data)
        {
            using var classbook = new ClassbookXlsxFileExport();

            await classbook.FillFileAsync(data);
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
            using var result = new StudentsResultsXlsxFileExport();

            await result.FillFileAsync(data);
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
            using var result = new StudentClassbookXlsxFileExport();

            await result.FillFileAsync(data);
            result.AdjustContent();

            return Result<FileDto>.GetSuccess(new FileDto
            {
                ByteArray = await result.GetByteArrayAsync(),
                ContentType = result.GetContentType(),
                Filename = result.GetFileName()
            });
        }

        public async Task<Result<FileDto>> GetStudentResultsAsync(StudentsResultsDto data)
        {
            using var result = new StudentResultXlsxFileExport();

            await result.FillFileAsync(data);
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
            using var result = new StudentGroupResultsXlsxFileExport();

            await result.FillFileAsync(data);
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

