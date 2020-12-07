using System;
using System.Text;
using System.Linq;
using CharlieBackend.Core;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Business.Services.Interfaces;
using System.Security.Cryptography.X509Certificates;
using CharlieBackend.Data.Repositories.Impl.Interfaces;

namespace CharlieBackend.Business.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<Result<StudentsClassbookResultDto>> GetStudentsClassbookAsync(StudentsRequestDto<ClassbookResultType> request)
        {
            var errors = ValidateStudentsRequest(request);

            if (errors.Any())
            {
                return Result<StudentsClassbookResultDto>.GetError(ErrorCode.ValidationError, 
                    String.Join(";\n", errors)) ;
            }

            var result = new StudentsClassbookResultDto();
            var studentsIds = new List<long>();

            if (request.StudentGroupId != default)
            {
                studentsIds = await _dashboardRepository
                    .GetStudentsIdsByGroupIdsAsync(new List<long> { (long)request.StudentGroupId });
            }
            else
            {
                var studentGroupsIds = await _dashboardRepository
                    .GetGroupsIdsByCourseIdAsync((long)request.CourseId, request.StartDate, request.FinishDate);

                studentsIds = await _dashboardRepository
                    .GetStudentsIdsByGroupIdsAsync(studentGroupsIds);
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

        public async Task<Result<StudentsResultsDto>> GetStudentsResultAsync(StudentsRequestDto<StudentResultType> request)
        {
            var errors = ValidateStudentsRequest(request);

            if (errors.Any())
            {
                return Result<StudentsResultsDto>.GetError(ErrorCode.ValidationError,
                    String.Join(";\n", errors));
            }

            var result = new StudentsResultsDto();
            var studentsIds = new List<long>();
            var studentGroupIds = new List<long>();

            if (request.StudentGroupId != default)
            {
                studentGroupIds.Add((long)request.StudentGroupId);

                studentsIds = await _dashboardRepository.GetStudentsIdsByGroupIdsAsync(
                    new List<long> { (long)request.StudentGroupId });
            }
            else
            {
                studentGroupIds = await _dashboardRepository
                    .GetGroupsIdsByCourseIdAsync((long)request.CourseId, request.StartDate, request.FinishDate);

                studentsIds = await _dashboardRepository
                    .GetStudentsIdsByGroupIdsAsync(studentGroupIds);
            }

            if (request.IncludeAnalytics.Contains(StudentResultType.AverageStudentMark))
            {
                result.AverageStudentsMarks = await _dashboardRepository
                    .GetStudentAverageMarksByStudentIdsAndGropsIdsAsync(studentsIds, studentGroupIds);
            }

            if (request.IncludeAnalytics.Contains(StudentResultType.AverageStudentVisits))
            {
                result.AverageStudentVisits = await _dashboardRepository
                    .GetStudentsAverageVisitsByStudentIdsAndGroupsIdsAsync(studentsIds, studentGroupIds);
            }

            return Result<StudentsResultsDto>.GetSuccess(result);
        }

        public async Task<Result<StudentsClassbookResultDto>> GetStudentClassbookAsync(long studentId,
            GenericRequestDto<ClassbookResultType> request, ClaimsPrincipal userContext)
        {
            var error = ValidateGenericRequest(request).Concat(ValidateStudentRights(studentId, userContext));

            if (error.Any())
            {
                return Result<StudentsClassbookResultDto>
                           .GetError(ErrorCode.ValidationError, String.Join(";\n",
                           error));
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
            GenericRequestDto<StudentResultType> request, ClaimsPrincipal userContext)
        {
            var errors = ValidateGenericRequest(request).Concat(ValidateStudentRights(studentId, userContext));

            if (errors.Any())
            {
                return Result<StudentsResultsDto>
                           .GetError(ErrorCode.ValidationError, String.Join(";\n", errors));
            }

            var result = new StudentsResultsDto();
            var studentGroupsIds = await _dashboardRepository
                .GetGroupsIdsByStudentIdAndPeriodAsync(studentId, request.StartDate, request.FinishDate);

            if (request.IncludeAnalytics.Contains(StudentResultType.AverageStudentMark))
            {
                result.AverageStudentsMarks = await _dashboardRepository
                    .GetStudentAverageMarksByStudentIdsAndGropsIdsAsync(new List<long> { studentId }, studentGroupsIds);
            }

            if (request.IncludeAnalytics.Contains(StudentResultType.AverageStudentVisits))
            {
                result.AverageStudentVisits = await _dashboardRepository
                    .GetStudentAverageVisitsPercentageByStudentIdsAsync(studentId, studentGroupsIds);
            }

            return Result<StudentsResultsDto>.GetSuccess(result);
        }

        public async Task<Result<StudentGroupsResultsDto>> GetStudentGroupResultAsync(long courseId,
            GenericRequestDto<StudentGroupResultType> request)
        {
            var errors = ValidateGenericRequest(request);

            if (errors.Any())
            {
                return Result<StudentGroupsResultsDto>.GetError(ErrorCode.ValidationError, String.Join(";\n",
                           errors));
            }

            var result = new StudentGroupsResultsDto();
            var studentGroupsIds = await _dashboardRepository
                    .GetGroupsIdsByCourseIdAsync(courseId, request.StartDate, request.FinishDate);

            if (studentGroupsIds == null)
            {
                return Result<StudentGroupsResultsDto>
                    .GetError(ErrorCode.NotFound, "Student groups not found");
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

        private IEnumerable<string> ValidateStudentsRequest<T>(StudentsRequestDto<T> request) where T: Enum
        {
            if (request == default)
            {
                yield return "Please provide request data";
                yield break;
            }

            if (request.IncludeAnalytics == default || request.IncludeAnalytics.Length == 0)
            {
                yield return "Please provide 'IncludeAnalytics' parameters";
            }

            if (!request.CourseId.HasValue && !request.StudentGroupId.HasValue)
            {
                yield return "Please provide course or student group id";
            }
        }

        private IEnumerable<string> ValidateStudentRights(long studentId, ClaimsPrincipal claimsContext)
        {
            if (studentId == default)
            {
                yield return "Please provide student id";
            }

            if (!IsRequestAllowedForStudent(studentId, claimsContext))
            {
                yield return "Not allowed to request other student results";
            }
        }

        private IEnumerable<string> ValidateGenericRequest<T>(GenericRequestDto<T> request) where T : Enum
        {
            if (request == default)
            {
                yield return "Please provide request data";
                yield break;
            }

            if (request.IncludeAnalytics == default || request.IncludeAnalytics.Length == 0)
            {
                yield return "Please provide 'IncludeAnalytics' parameters";
            }
        }

        private bool IsRequestAllowedForStudent(long studentId, ClaimsPrincipal claimsContext)
        {
            var isStudentRole = claimsContext.IsInRole(UserRole.Student.ToString());

            if (isStudentRole)
            {
                var studentIdFromContext = Convert.ToInt64(claimsContext.Claims
                    .First(claim => claim.Type == "Id").Value);

                if (studentId != studentIdFromContext)
                {
                    return false;
                }
            }

                return true;
        }
    }
}

