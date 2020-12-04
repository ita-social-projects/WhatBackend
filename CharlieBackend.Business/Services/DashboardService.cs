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
using CharlieBackend.Core.DTO.Dashboard.StudentClassbook;

namespace CharlieBackend.Business.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IUnitOfWork unitOfWork, IDashboardRepository dashboardRepository)
        {
            _unitOfWork = unitOfWork;
            _dashboardRepository = dashboardRepository;
        }

        public async Task<Result<StudentsClassbookResultDto>> GetStudentsClassbookAsync(StudentsClassbookRequestDto request)
        {
            if (ValidateStudentsClassbookRequest(request).Any())
            {
                return Result<StudentsClassbookResultDto>.GetError(ErrorCode.ValidationError, 
                    String.Join(";\n", ValidateStudentsClassbookRequest(request))) ;
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

        public async Task<Result<StudentsResultsDto>> GetStudentsResultAsync(StudentsResultsRequestDto request)
        {
            if (ValidateStudentsResultsRequest(request).Any())
            {
                return Result<StudentsResultsDto>.GetError(ErrorCode.ValidationError,
                    String.Join(";\n", ValidateStudentsResultsRequest(request)));
            }

            var result = new StudentsResultsDto();
            var studentsIds = new List<long>();

            if (request.StudentGroupId != default)
            {
                studentsIds = await _dashboardRepository.GetStudentsIdsByGroupIdsAsync(
                    new List<long> { (long)request.StudentGroupId });
            }
            else
            {
                var studentGroupsIds = await _dashboardRepository
                    .GetGroupsIdsByCourseIdAsync((long)request.CourseId, request.StartDate, request.FinishtDate);

                studentsIds = await _dashboardRepository
                    .GetStudentsIdsByGroupIdsAsync(studentGroupsIds);
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
            StudentClassbookRequestDto request, ClaimsPrincipal userContext)
        {
            if (ValidateStudentClassbookRequest(studentId, request, userContext).Any())
            {
                return Result<StudentsClassbookResultDto>
                           .GetError(ErrorCode.ValidationError, String.Join(";\n",
                           ValidateStudentClassbookRequest(studentId, request, userContext)));
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
            StudentResultRequestDto request, ClaimsPrincipal userContext)
        {
            if (ValidateStudentResultRequest(studentId, request, userContext).Any())
            {
                return Result<StudentsResultsDto>
                           .GetError(ErrorCode.ValidationError, String.Join(";\n",
                           ValidateStudentResultRequest(studentId, request, userContext)));
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

        public async Task<Result<StudentGroupsResultsDto>> GetStudentGroupResultAsync(long courseId, 
            StudentGroupsResultsRequestDto request)
        {
            if (ValidateStudentGroupsResultsRequest(courseId, request).Any())
            {
                return Result<StudentGroupsResultsDto>
                           .GetError(ErrorCode.ValidationError, String.Join(";\n",
                           ValidateStudentGroupsResultsRequest(courseId, request)));
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

        private IEnumerable<string> ValidateStudentsClassbookRequest(StudentsClassbookRequestDto request)
        {
            if (request == default)
            {
                yield return "Please provide request data";
                yield break;
            }

            if (request.IncludeAnalytics == default)
            {
                yield return "Please provide 'IncludeAnalytics' data";
            }

            if (request.IncludeAnalytics.Length == 0)
            {
                yield return "Please provide 'IncludeAnalytics' parameters";
            }

            if (!request.CourseId.HasValue && !request.StudentGroupId.HasValue)
            {
                yield return "Please provide course or student group id";
            }
        }

        private IEnumerable<string> ValidateStudentsResultsRequest(StudentsResultsRequestDto request)
        {
            if (request == default)
            {
                yield return "Please provide request data";
                yield break;
            }

            if (request.IncludeAnalytics == default)
            {
                yield return "Please provide 'IncludeAnalytics' parameters";
            }

            if (request.IncludeAnalytics.Length == 0)
            {
                yield return "Please provide 'IncludeAnalytics' parameters";
            }

            if (!request.CourseId.HasValue && !request.StudentGroupId.HasValue)
            {
                yield return "Please provide course or student group id";
            }
        }

        private IEnumerable<string> ValidateStudentClassbookRequest(long studentId,
            StudentClassbookRequestDto request, ClaimsPrincipal claimsContext)
        {
            if (studentId == default)
            {
                yield return "Please provide student id";
            }

            if (request == default)
            {
                yield return "Please provide request data";
                yield break;
            }

            if (request.IncludeAnalytics == default)
            {
                yield return "Please provide 'IncludeAnalytics' parameters";
            }

            if (request.IncludeAnalytics.Length == 0)
            {
                yield return "Please provide 'IncludeAnalytics' parameters";
            }

            var isStudentRole = claimsContext.IsInRole("Student");

            if (!IsRequestAllowedForStudent(studentId, claimsContext))
            {
                yield return "Not allowed to request other student results";
            }
        }

        private IEnumerable<string> ValidateStudentResultRequest(long studentId,
    StudentResultRequestDto request, ClaimsPrincipal claimsContext)
        {
            if (studentId == default)
            {
                yield return "Please provide student id";
            }

            if (request == default)
            {
                yield return "Please provide request data";
                yield break;
            }

            if (request.IncludeAnalytics == default)
            {
                yield return "Please provide 'IncludeAnalytics' parameters";
            }

            if (request.IncludeAnalytics.Length == 0)
            {
                yield return "Please provide 'IncludeAnalytics' parameters";
            }

            var isStudentRole = claimsContext.IsInRole("Student");

            if (!IsRequestAllowedForStudent(studentId, claimsContext))
            {
                yield return "Not allowed to request other student results";
            }
        }

        private IEnumerable<string> ValidateStudentGroupsResultsRequest(long courseId, StudentGroupsResultsRequestDto request)
        {
            if (courseId == default)
            {
                yield return "Please provide course id";
            }

            if (request == default)
            {
                yield return "Please provide request data";
                yield break;
            }

            if (request.IncludeAnalytics == default)
            {
                yield return "Please provide 'IncludeAnalytics' parameters";
            }

            if (request.IncludeAnalytics.Length == 0)
            {
                yield return "Please provide 'IncludeAnalytics' parameters";
            }
        }

        private bool IsRequestAllowedForStudent(long studentId, ClaimsPrincipal claimsContext)
        {
            var isStudentRole = claimsContext.IsInRole("Student");

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

