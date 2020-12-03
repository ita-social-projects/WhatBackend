using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Core.DTO.Dashboard.StudentClassbook;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IDashboardService
    {
        public Task<Result<StudentsClassbookResultDto>> GetStudentsClassbookAsync(StudentsClassbookRequestDto request);

        public Task<Result<StudentsResultsDto>> GetStudentsResultAsync(StudentsResultsRequestDto request);

        public Task<Result<StudentsClassbookResultDto>> GetStudentClassbookAsync(long studentId, StudentClassbookRequestDto request, string authHeader);

        public Task<Result<StudentsResultsDto>> GetStudentResultAsync(long studentId, StudentResultRequestDto request, string authHeader);

        public Task<Result<StudentGroupsResultsDto>> GetStudentGroupResultAsync(long courseId, StudentGroupsResultsRequestDto request);
    }
}
