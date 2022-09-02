using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DashboardService(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<StudentsClassbookResultDto>> GetStudentsClassbookAsync(StudentsRequestDto<ClassbookResultType> request)
        {
            var errors = ValidateStudentsRequest(request);

            if (errors.Any())
            {
                return Result<StudentsClassbookResultDto>.GetError(ErrorCode.ValidationError, string.Join(";\n", errors));
            }

            StudentsClassbookResultDto result = new StudentsClassbookResultDto();

            var studentGroupsIds = request.StudentGroupId.HasValue && request.StudentGroupId.Value != 0
                ? new List<long> { request.StudentGroupId.Value }
                : await _unitOfWork.DashboardRepository
                    .GetGroupsIdsByCourseIdAndPeriodAsync(request.CourseId.Value, request.StartDate, request.FinishDate);
            
            if (request.IncludeAnalytics.Contains(ClassbookResultType.StudentPresence))
            {
                result.StudentsPresences = await _unitOfWork.DashboardRepository
                    .GetStudentsPresenceListByGroupIdsAndDate(studentGroupsIds, request.StartDate, request.FinishDate);
            }

            if (request.IncludeAnalytics.Contains(ClassbookResultType.StudentMarks))
            {
                result.StudentsMarks = await _unitOfWork.DashboardRepository
                    .GetStudentsMarksListByGroupIdsAndDate(studentGroupsIds, request.StartDate, request.FinishDate);
            }

            return Result<StudentsClassbookResultDto>.GetSuccess(result);
        }

        public async Task<Result<StudentsResultsDto>> GetStudentsResultAsync(StudentsRequestDto<StudentResultType> request)
        {
            var errors = ValidateStudentsRequest(request);

            if (errors.Any())
            {
                return Result<StudentsResultsDto>.GetError(ErrorCode.ValidationError, string.Join(";\n", errors));
            }

            var result = new StudentsResultsDto();
            var studentGroupsIds = request.StudentGroupId.HasValue
                ? new List<long> { request.StudentGroupId.Value }
                : await _unitOfWork.DashboardRepository
                    .GetGroupsIdsByCourseIdAndPeriodAsync(request.CourseId.Value, request.StartDate, request.FinishDate);

            var studentsIds = await _unitOfWork.DashboardRepository.GetStudentsIdsByGroupIdsAsync(studentGroupsIds);

            if (request.IncludeAnalytics.Contains(StudentResultType.AverageStudentMark))
            {
                result.AverageStudentsMarks = await _unitOfWork.DashboardRepository
                    .GetStudentAvgMarksAsync(studentsIds, studentGroupsIds);
            }

            if (request.IncludeAnalytics.Contains(StudentResultType.AverageStudentVisits))
            {
                result.AverageStudentVisits = await _unitOfWork.DashboardRepository
                    .GetStudentsAverageVisitsByStudentIdsAndGroupsIdsAsync(studentsIds, studentGroupsIds);
            }

            if (request.IncludeAnalytics.Contains(StudentResultType.AverageStudentHomeworkMark))
            {
                result.AverageStudentHomeworkMarks = await _unitOfWork.DashboardRepository.GetStudentHomeworkAvgMarksAsync(studentsIds, studentGroupsIds);
            }

            return Result<StudentsResultsDto>.GetSuccess(result);
        }

        public async Task<Result<StudentsClassbookResultDto>> GetStudentClassbookAsync(long studentId,
            DashboardAnalyticsRequestDto<ClassbookResultType> request)
        {
            var error = ValidateGenericRequest(request).Concat(ValidateStudentRights(studentId));

            if (error.Any())
            {
                return Result<StudentsClassbookResultDto>
                    .GetError(ErrorCode.ValidationError, string.Join(";\n", error));
            }

            var result = new StudentsClassbookResultDto();
            var studentGroupsIds = await _unitOfWork.DashboardRepository
                .GetGroupsIdsByStudentIdAndPeriodAsync(studentId, request.StartDate, request.FinishDate);

            if (request.IncludeAnalytics.Contains(ClassbookResultType.StudentPresence))
            {
                result.StudentsPresences = await _unitOfWork.DashboardRepository
                    .GetStudentPresenceListByStudentIds(studentId, studentGroupsIds);
            }

            if (request.IncludeAnalytics.Contains(ClassbookResultType.StudentMarks))
            {
                result.StudentsMarks = await _unitOfWork.DashboardRepository
                    .GetStudentMarksListByStudentIds(studentId, studentGroupsIds);
            }

            return Result<StudentsClassbookResultDto>.GetSuccess(result);
        }

        public async Task<Result<StudentsResultsDto>> GetStudentResultAsync(long studentId,
            DashboardAnalyticsRequestDto<StudentResultType> request)
        {
            var errors = ValidateGenericRequest(request).Concat(ValidateStudentRights(studentId));

            if (errors.Any())
            {
                return Result<StudentsResultsDto>.GetError(ErrorCode.ValidationError, string.Join(";\n", errors));
            }

            var result = new StudentsResultsDto();
            var studentGroupsIds = await _unitOfWork.DashboardRepository
                .GetGroupsIdsByStudentIdAndPeriodAsync(studentId, request.StartDate, request.FinishDate);

            if (request.IncludeAnalytics.Contains(StudentResultType.AverageStudentMark))
            {
                result.AverageStudentsMarks = await _unitOfWork.DashboardRepository
                    .GetStudentAvgMarksAsync(new List<long> { studentId }, studentGroupsIds);
            }

            if (request.IncludeAnalytics.Contains(StudentResultType.AverageStudentVisits))
            {
                result.AverageStudentVisits = await _unitOfWork.DashboardRepository
                    .GetStudentAvgVisitsPercentageAsync(studentId, studentGroupsIds);
            }

            if(request.IncludeAnalytics.Contains(StudentResultType.AverageStudentHomeworkMark))
            {
                result.AverageStudentHomeworkMarks = await _unitOfWork.DashboardRepository.GetStudentHomeworkAvgMarksAsync(new List<long> { studentId }, studentGroupsIds);
            }

            return Result<StudentsResultsDto>.GetSuccess(result);
        }

        public async Task<Result<StudentGroupsResultsDto>> GetStudentGroupResultAsync(long courseId,
            DashboardAnalyticsRequestDto<StudentGroupResultType> request)
        {
            var errors = ValidateGenericRequest(request);

            if (errors.Any())
            {
                return Result<StudentGroupsResultsDto>.GetError(ErrorCode.ValidationError, string.Join(";\n",
                           errors));
            }

            var result = new StudentGroupsResultsDto();
            var studentGroupsIds = await _unitOfWork.DashboardRepository
                    .GetGroupsIdsByCourseIdAndPeriodAsync(courseId, request.StartDate, request.FinishDate);

            if (studentGroupsIds == null)
            {
                return Result<StudentGroupsResultsDto>
                    .GetError(ErrorCode.NotFound, "Student groups not found");
            }

            if (request.IncludeAnalytics.Contains(StudentGroupResultType.AverageStudentGroupMark))
            {
                result.AverageStudentGroupsMarks = await _unitOfWork.DashboardRepository
                    .GetStudentGroupsAverageMarks(studentGroupsIds);
            }

            if (request.IncludeAnalytics.Contains(StudentGroupResultType.AverageStudentGroupVisitsPercentage))
            {
                result.AverageStudentGroupsVisits = await _unitOfWork.DashboardRepository
                    .GetStudentGroupsAverageVisits(studentGroupsIds);
            }

            return Result<StudentGroupsResultsDto>.GetSuccess(result);
        }

        private IEnumerable<string> ValidateStudentsRequest<T>(StudentsRequestDto<T> request) 
            where T : Enum
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

            if ((!request.CourseId.HasValue || request.CourseId == default) 
                && (!request.StudentGroupId.HasValue || request.StudentGroupId == default))
            {
                yield return "Please provide course or student group id";
            }
        }

        private IEnumerable<string> ValidateStudentRights(long studentId)
        {
            if (studentId == default)
            {
                yield return "Please provide student id";
            }

            if (!IsRequestAllowedForStudent(studentId))
            {
                yield return "Not allowed to request other student results";
            }
        }

        private IEnumerable<string> ValidateGenericRequest<T>(DashboardAnalyticsRequestDto<T> request) where T : Enum
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

        private bool IsRequestAllowedForStudent(long studentId)
        {
            var isStudentRole = _currentUserService.Role == UserRole.Student;

            if (isStudentRole)
            {
                var currentUserEntityId = _currentUserService.EntityId;

                if (studentId != currentUserEntityId)
                {
                    return false;
                }
            }

            return true;
        }
    }
}

