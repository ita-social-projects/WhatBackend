using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Core.DTO.Dashboard;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IDashboardService
    {
        public Task<Result<StudentsClassbookResultDto>> GetStudentsClassbookAsync(long courceId, long groupId, StudentsClassbookRequestDto request);

        public Task<Result<StudentsResultsDto>> GetStudentsResultAsync(long courceId, long groupId, StudentsResultsRequestDto request);

        public Task<Result<StudentsClassbookResultDto>> GetStudentClassbookAsync(long studentId, StudentsClassbookRequestDto request, string authHeader);

        public Task<Result<StudentsResultsDto>> GetStudentResultAsync(long studentId, StudentsResultsRequestDto request, string authHeader);

        public Task<Result<StudentGroupsResultsDto>> GetStudentGroupResultAsync(long courceId, StudentGroupsResultsRequestDto request);
    }
}
