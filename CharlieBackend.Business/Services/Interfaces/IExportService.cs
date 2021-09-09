using CharlieBackend.Business.Services.FileServices.ExportFileServices;
using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Core.DTO.Export;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IExportService
    {
        Task<FileDto> GetStudentsClassbook(StudentsClassbookResultDto data);

        Task<FileDto> GetStudentsResults(StudentsResultsDto data);

        Task<FileDto> GetStudentClassbook(StudentsClassbookResultDto data);

        Task<FileDto> GetStudentResults(StudentsResultsDto data);

        Task<FileDto> GetStudentGroupResults(StudentGroupsResultsDto data);
    }
}
