using System;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Core.Models.ResultModel;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<Result<StudentsClassbookResultDto>> GetStudentsClassbookAsync(StudentsRequestDto<ClassbookResultType> request);

        Task<Result<StudentsResultsDto>> GetStudentsResultAsync(StudentsRequestDto<StudentResultType> request);

        Task<Result<StudentsClassbookResultDto>> GetStudentClassbookAsync(long studentId, DashboardAnalyticsRequestDto<ClassbookResultType> request, ClaimsPrincipal userContext);

        Task<Result<StudentsResultsDto>> GetStudentResultAsync(long studentId, DashboardAnalyticsRequestDto<StudentResultType> request, ClaimsPrincipal userContext);

        Task<Result<StudentGroupsResultsDto>> GetStudentGroupResultAsync(long courseId, DashboardAnalyticsRequestDto<StudentGroupResultType> request);
    }
}
