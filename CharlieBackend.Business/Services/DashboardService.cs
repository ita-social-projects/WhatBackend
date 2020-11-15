using System;
using System.Text;
using System.Collections.Generic;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Core.Models.ResultModel;
using System.Threading.Tasks;
using CharlieBackend.Core;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using CharlieBackend.Core.Entities;

namespace CharlieBackend.Business.Services
{
    public class DashboardService : IDashboardService
    {
        #region privateFields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDashboardRepository _dashboardRepository;
        #endregion

        public DashboardService(IUnitOfWork unitOfWork, IDashboardRepository dashboardRepository)
        {
            _unitOfWork = unitOfWork;
            _dashboardRepository = dashboardRepository;
        }

        public async Task<Result<DashboardResultDto>> GetStudentsResultAsync(DashboardRequestDto request)
        {
            if (request == null && request.IncludeAnalytics == null)
            {
                return Result<DashboardResultDto>
                    .Error(ErrorCode.UnprocessableEntity, "No request data parameters given");
            }

            var result = new DashboardResultDto();
            var studentsIds = new List<long>();

            if (request.CourseId != null && request.CourseId != 0)
            {
                var studentGroupsIds = await _dashboardRepository
                    .GetGroupsIdsByCourceIdAsync((long)request.CourseId, request.StartDate);

                studentsIds = await _dashboardRepository
                    .GetStudentsIdsByGroupIdsAsync(studentGroupsIds);
            }
            else if (request.GroupId != null && request.CourseId != 0)
            {
                studentsIds = await _dashboardRepository.GetStudentsIdsByGroupIdAsync((long)request.GroupId);
            }
            else
            {
                return Result<DashboardResultDto>
                    .Error(ErrorCode.UnprocessableEntity, "No request data parameters given");
            }

            if (request.IncludeAnalytics.Contains(DashboardResultType.AverageStudentMark))
            {
                result.AverageStudentsMarks = await _dashboardRepository
                    .GetStudentsAverageMarksByStudentIdsAsync(studentsIds);
            }

            if (request.IncludeAnalytics.Contains(DashboardResultType.AverageStudentVisits))
            {
                result.AverageStudentVisits = await _dashboardRepository
                    .GetStudentsAverageVisitsByStudentIdsAsync(studentsIds);

            }

            if (request.IncludeAnalytics.Contains(DashboardResultType.StudentPresence))
            {
                result.StudentsPresences = await _dashboardRepository
                    .GetStudentsPresenceListByStudentIds(studentsIds);
            }

            if (request.IncludeAnalytics.Contains(DashboardResultType.StudentMarks))
            {
                result.StudentsMarks = await _dashboardRepository
                    .GetStudentsMarksListByStudentIds(studentsIds);
            }

            return Result<DashboardResultDto>.Success(result);
        }

        private IEnumerable<string> ValidateAverageStudentMarkAnalytics(DashboardRequestDto request)
        {
            if (!request.GroupId.HasValue)
            {
                yield return "Please provide groupId";
            }
        }
    }
}

