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
        public Task<Result<StudentsClassbookResultDto>> GetStudentsClassbookAsync(StudentsRequestDto<ClassbookResultType> request);

        public Task<Result<StudentsResultsDto>> GetStudentsResultAsync(StudentsRequestDto<StudentResultType> request);

        public Task<Result<StudentsClassbookResultDto>> GetStudentClassbookAsync(long studentId, GenericRequestDto<ClassbookResultType> request, ClaimsPrincipal userContext);

        public Task<Result<StudentsResultsDto>> GetStudentResultAsync(long studentId, GenericRequestDto<StudentResultType> request, ClaimsPrincipal userContext);

        public Task<Result<StudentGroupsResultsDto>> GetStudentGroupResultAsync(long courseId, GenericRequestDto<StudentGroupResultType> request);
    }
}
