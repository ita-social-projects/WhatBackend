using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Core.DTO.Export;
using CharlieBackend.Core.Models.ResultModel;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices
{
    public interface IExportService
    {
        Task<Result<FileDto>> GetStudentsClassbook(StudentsClassbookResultDto data);

        Task<Result<FileDto>> GetStudentsResults(StudentsResultsDto data);

        Task<Result<FileDto>> GetStudentClassbook(StudentsClassbookResultDto data);

        Task<Result<FileDto>> GetStudentResults(StudentsResultsDto data);

        Task<Result<FileDto>> GetStudentGroupResults(StudentGroupsResultsDto data);

        Task<Result<FileDto>> GetListofStudentsByGroupId(long groupId);
    }
}
