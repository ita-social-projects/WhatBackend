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
        public Task<Result<IList<StudentGroupResultDto>>> GetResultAsync(DashboardRequestDto request);

        public Task<Result<IList<StudentGroupResultDto>>> GetGroupsListByCourceIdAsync(long courceId, params DashboardResultType[] type);

        public Task<Result<StudentGroupResultDto>> GetStudentGroupResultsAsync(long studentGroupId, params DashboardResultType[] type);

        public Task<Result<StudentResultsOfStudentGroupDto>> GetStudentOfGroupResultsAsync(long studentId,
                    long studentGroupId, params DashboardResultType[] type);

        public Task<Result<double?>> GetStudentOfGroupAverageMarkAsync(long studentId, long studentGroupId);

        public Task<Result<int?>> GetStudentAverageVisitsResultsAsync(long studentId, long studentGroupId);

        public Task<Result<IList<LessonResultDto>>> GetStudentClassBookResultsAsync(long studentId, long studentGroupId);

    }
}
