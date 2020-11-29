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
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

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

        public async Task<Result<StudentsClassbookResultDto>> GetStudentsClassbookAsync(long courceId, long groupId,
                StudentsClassbookRequestDto request)
        {
            if (request == default && request.IncludeAnalytics == default)
            {
                return Result<StudentsClassbookResultDto>
                    .GetError(ErrorCode.UnprocessableEntity, "No request data parameters given");
            }

            if (courceId == default(long) && groupId == default(long))
            {
                return Result<StudentsClassbookResultDto>
                    .GetError(ErrorCode.UnprocessableEntity, "No course or group Id given");
            }

            var result = new StudentsClassbookResultDto();
            var studentsIds = new List<long>();

            if (courceId != default)
            {
                var studentGroupsIds = await _dashboardRepository
                    .GetGroupsIdsByCourceIdAsync(courceId, request.StartDate, request.FinishDate);

                studentsIds = await _dashboardRepository
                    .GetStudentsIdsByGroupIdsAsync(studentGroupsIds);
            }
            else if (groupId != default)
            {
                studentsIds = await _dashboardRepository.GetStudentsIdsByGroupIdAsync(groupId);
            }
            else
            {
                return Result<StudentsClassbookResultDto>
                    .GetError(ErrorCode.UnprocessableEntity, "No correct request parameters given");
            }

            if (request.IncludeAnalytics.Contains(ClassbookResultType.StudentPresence))
            {
                result.StudentsPresences = await _dashboardRepository
                    .GetStudentsPresenceListByStudentIds(studentsIds);
            }

            if (request.IncludeAnalytics.Contains(ClassbookResultType.StudentMarks))
            {
                result.StudentsMarks = await _dashboardRepository
                    .GetStudentsMarksListByStudentIds(studentsIds);
            }

            return Result<StudentsClassbookResultDto>.GetSuccess(result);
        }

        public async Task<Result<StudentsResultsDto>> GetStudentsResultAsync(long courceId, long groupId, 
            StudentsResultsRequestDto request)
        {
            if (request == null && request.IncludeAnalytics == null)
            {
                return Result<StudentsResultsDto>
                    .GetError(ErrorCode.UnprocessableEntity, "No request data parameters given");
            }

            if (courceId == default(long) && groupId == default(long))
            {
                return Result<StudentsResultsDto>
                    .GetError(ErrorCode.UnprocessableEntity, "No course or group Id given");
            }

            var result = new StudentsResultsDto();
            var studentsIds = new List<long>();

            if (courceId != default(long))
            {
                var studentGroupsIds = await _dashboardRepository
                    .GetGroupsIdsByCourceIdAsync(courceId, request.StartDate, request.FinishtDate);

                studentsIds = await _dashboardRepository
                    .GetStudentsIdsByGroupIdsAsync(studentGroupsIds);
            }
            else if (groupId != default(long))
            {
                studentsIds = await _dashboardRepository.GetStudentsIdsByGroupIdAsync(groupId);
            }
            else
            {
                return Result<StudentsResultsDto>
                    .GetError(ErrorCode.UnprocessableEntity, "No request data parameters given");
            }

            if (request.IncludeAnalytics.Contains(StudentResultType.AverageStudentMark))
            {
                result.AverageStudentsMarks = await _dashboardRepository
                    .GetStudentsAverageMarksByStudentIdsAsync(studentsIds);
            }

            if (request.IncludeAnalytics.Contains(StudentResultType.AverageStudentVisits))
            {
                result.AverageStudentVisits = await _dashboardRepository
                    .GetStudentsAverageVisitsByStudentIdsAsync(studentsIds);
            }

            return Result<StudentsResultsDto>.GetSuccess(result);
        }

        public async Task<Result<StudentsClassbookResultDto>> GetStudentClassbookAsync(long studentId, 
            StudentsClassbookRequestDto request, string authHeader)
        {
            var handler = new JwtSecurityTokenHandler();
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
            var role = tokenS.Claims.First(claim => claim.Type == ClaimsIdentity.DefaultRoleClaimType).Value;

            if (request == null && request.IncludeAnalytics == null && studentId == 0)
            {
                return Result<StudentsClassbookResultDto>
                    .GetError(ErrorCode.UnprocessableEntity, "No request data parameters given");
            }

            if (role == "Student")
            {
                var studentIdfromToken = Convert.ToInt64(tokenS.Claims.First(claim => claim.Type == "Id").Value);

                if (studentIdfromToken != default && studentIdfromToken != studentId)
                {
                    return Result<StudentsClassbookResultDto>
                        .GetError(ErrorCode.Unauthorized, "Not allowed to get other student results");
                }
            }

            var result = new StudentsClassbookResultDto();
            var studentGroupsIds = await _dashboardRepository
                .GetGroupsIdsByStudentIdAndPeriodAsync(studentId, request.StartDate, request.FinishDate);

            if (request.IncludeAnalytics.Contains(ClassbookResultType.StudentPresence))
            {
                result.StudentsPresences = await _dashboardRepository
                    .GetStudentPresenceListByStudentIds(studentId, studentGroupsIds);
            }

            if (request.IncludeAnalytics.Contains(ClassbookResultType.StudentMarks))
            {
                result.StudentsMarks = await _dashboardRepository
                    .GetStudentMarksListByStudentIds(studentId, studentGroupsIds);
            }

            return Result<StudentsClassbookResultDto>.GetSuccess(result);
        }

        public async Task<Result<StudentsResultsDto>> GetStudentResultAsync(long studentId, 
            StudentsResultsRequestDto request, string authHeader)
        {
            var handler = new JwtSecurityTokenHandler();
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
            var role = tokenS.Claims.First(claim => claim.Type == ClaimsIdentity.DefaultRoleClaimType).Value;

            if (request == null && request.IncludeAnalytics == null && studentId == 0)
            {
                return Result<StudentsResultsDto>
                    .GetError(ErrorCode.UnprocessableEntity, "No request data parameters given");
            }

            if (role == "Student")
            {
                var studentIdfromToken = Convert.ToInt64(tokenS.Claims.First(claim => claim.Type == "Id").Value);

                if (studentIdfromToken != default && studentIdfromToken != studentId)
                {
                    return Result<StudentsResultsDto>
                        .GetError(ErrorCode.Unauthorized, "Not allowed to get other student results");
                }
            }

            var result = new StudentsResultsDto();
            var studentGroupsIds = await _dashboardRepository
                .GetGroupsIdsByStudentIdAndPeriodAsync(studentId, request.StartDate, request.FinishtDate);

            if (request.IncludeAnalytics.Contains(StudentResultType.AverageStudentMark))
            {
                result.AverageStudentsMarks = await _dashboardRepository
                    .GetStudentAverageMarksByStudentIdAsync(studentId, studentGroupsIds);
            }

            if (request.IncludeAnalytics.Contains(StudentResultType.AverageStudentVisits))
            {
                result.AverageStudentVisits = await _dashboardRepository
                    .GetStudentAverageVisitsPercentageByStudentIdsAsync(studentId, studentGroupsIds);
            }

            return Result<StudentsResultsDto>.GetSuccess(result);
        }

        public async Task<Result<StudentGroupsResultsDto>> GetStudentGroupResultAsync(long courceId, StudentGroupsResultsRequestDto request)
        {
            if (courceId == 0 && request == null && request.IncludeAnalytics == null)
            {
                return Result<StudentGroupsResultsDto>
                    .GetError(ErrorCode.UnprocessableEntity, "No request data parameters given");
            }

            var result = new StudentGroupsResultsDto();
            var studentGroupsIds = await _dashboardRepository
                    .GetGroupsIdsByCourceIdAsync(courceId, request.StartDate, request.FinishDate);

            if (studentGroupsIds == null)
            {
                return Result<StudentGroupsResultsDto>.GetSuccess(result);
            }

            if (request.IncludeAnalytics.Contains(StudentGroupResultType.AverageStudentGroupMark))
            {
                result.AverageStudentGroupsMarks = await _dashboardRepository
                    .GetStudentGroupsAverageMarks(studentGroupsIds);
            }

            if (request.IncludeAnalytics.Contains(StudentGroupResultType.AverageStudentGroupVisitsPercentage))
            {
                result.AverageStudentGroupsVisits = await _dashboardRepository
                    .GetStudentGroupsAverageVisits(studentGroupsIds);
            }

            return Result<StudentGroupsResultsDto>.GetSuccess(result);
        }

        //private IEnumerable<string> ValidateAverageStudentMarkAnalytics(StudentsResultsRequestDto request)
        //{
        //    if (!request.GroupId.HasValue)
        //    {
        //        yield return "Please provide groupId";
        //    }
        //}
    }
}

