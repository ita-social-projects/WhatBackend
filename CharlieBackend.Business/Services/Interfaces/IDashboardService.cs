using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Core.Models.ResultModel;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<Result<StudentsClassbookResultDto>> GetStudentsClassbookAsync(StudentsRequestDto<ClassbookResultType> request);

        Task<Result<StudentsResultsDto>> GetStudentsResultAsync(StudentsRequestDto<StudentResultType> request);

        Task<Result<StudentsClassbookResultDto>> GetStudentClassbookAsync(long studentId, DashboardAnalyticsRequestDto<ClassbookResultType> request);

        Task<Result<StudentsResultsDto>> GetStudentResultAsync(long studentId, DashboardAnalyticsRequestDto<StudentResultType> request);

        Task<Result<StudentGroupsResultsDto>> GetStudentGroupResultAsync(long courseId, DashboardAnalyticsRequestDto<StudentGroupResultType> request);
    }
}
