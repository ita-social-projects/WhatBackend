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
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IList<StudentGroupResultDto>>> GetResultAsync(DashboardRequestDto request)
        {
            if (request == null)
            {
                return Result<IList<StudentGroupResultDto>>
                    .Error(ErrorCode.UnprocessableEntity, "No request data parameters given");
            }

            var result = new DashboardDto();
            if (request.IncludeAnalytics.Contains(DashboardResultType.AverageStudentMark))
            {
                ValidateAverageStudentMarkAnalytics(request);
                //data aggregation
                //var groupIds = await _unitOfWork.StudentGroupRepository
                //.GetQueryableNoTracking()
                //.Where(x => x.CourseId == courseId)
                //.Select(x => x.CourseId)
                //.ToListAsync();

                //var studentsByGroups = await _unitOfWork.StudentRepository
                //    .GetQueryableNoTracking()
                //    .Where(x => x.StudentsOfStudentGroups.Any(x => x.StudentGroupId.HasValue && groupIds.Contains(x.StudentGroupId.Value)))
                //    .ToListAsync();
                //...
                //result.AverageMarks = 
            }

            if (request.IncludeAnalytics.Contains(DashboardResultType.AverageStudentMark))
            {
                ValidateAverageStudentMarkAnalytics(request);
                //data aggregation
                //...
                //result.AverageMarks = 
            }








            if (request.CourceId != null && request.CourceId != 0)
            {
                var groupsResults =  await GetGroupsListByCourceIdAsync((long)request.CourceId, request.IncludeAnalytics);

                if (groupsResults.Data != null)
                { 
                return Result<IList<StudentGroupResultDto>>.Success(groupsResults.Data);
                }
                else
                {
                    return Result<IList<StudentGroupResultDto>>
                    .Error(ErrorCode.NotFound, "No groups found");
                }
            }

            if (request.GroupId != null && request.GroupId != 0)
            {
                var groupResult = await GetStudentGroupResultsAsync((long)request.GroupId, request.IncludeAnalytics);

                if (groupResult.Data != null)
                { 
                var groupsResult = new List<StudentGroupResultDto>() { groupResult.Data };

                return Result<IList<StudentGroupResultDto>>.Success(groupsResult);
                }
            }

            return Result<IList<StudentGroupResultDto>>
                    .Error(ErrorCode.UnprocessableEntity, "Request parameters not recognised");
        }

        private IEnumerable<string> ValidateAverageStudentMarkAnalytics(DashboardRequestDto request)
        {
            if (!request.GroupId.HasValue)
            {
                yield return "Please provide groupId";
            }
        }

        public async Task<Result<IList<StudentGroupResultDto>>> GetGroupsListByCourceIdAsync(long courseId, params DashboardResultType[] type)
        {
            var groupIds = await _unitOfWork.StudentGroupRepository
                .GetQueryableNoTracking()
                .Where(x => x.CourseId == courseId)
                .Select(x => x.CourseId)
                .ToListAsync();

            var studentsByGroups = await _unitOfWork.StudentRepository
                .GetQueryableNoTracking()
                .Where(x => x.StudentsOfStudentGroups.Any(x => x.StudentGroupId.HasValue && groupIds.Contains(x.StudentGroupId.Value)))
                .Select(x => new {
                    x.AccountId,
                    x.Id
                })
                .ToListAsync();



            var groupsList = await _unitOfWork.DashboardRepository
                    .GetStudentGroupsByCourceIdAsync(courseId);

            if (groupsList != null)
            {
                var studentGroupsResults = new List<StudentGroupResultDto>();

                for (int groupNumber = 0; groupNumber < groupsList.Count; groupNumber++)
                {
                    var studentGroupResult = new StudentGroupResultDto();

                    studentGroupResult.StudentGroupId = groupsList[groupNumber].Id;
                    studentGroupResult.CourceId = groupsList[groupNumber].CourseId;

                    var studentsOfStudentGroup = await _unitOfWork.DashboardRepository
                        .GetStudentsOfStudentGroup(studentGroupResult.StudentGroupId);

                    if (studentsOfStudentGroup != null)
                    {
                        studentGroupResult.StudentsResultsOfStudentGroup = new List<StudentResultsOfStudentGroupDto>();

                        for (int studentNumber = 0; studentNumber < studentsOfStudentGroup.Count; studentNumber++)
                        {
                            var studentResultsOfStudentGroupDto = await GetStudentOfGroupResultsAsync(
                                studentsOfStudentGroup[studentNumber].Id, groupsList[groupNumber].Id, type);

                            studentGroupResult.StudentsResultsOfStudentGroup.Add(studentResultsOfStudentGroupDto.Data);
                        }
                    }

                    studentGroupsResults.Add(studentGroupResult);
                }

                return Result<IList<StudentGroupResultDto>>.Success(studentGroupsResults);
            }

            return Result<IList<StudentGroupResultDto>>.Error(ErrorCode.NotFound, "No groups found on course");
        }

        public async Task<Result<StudentGroupResultDto>> GetStudentGroupResultsAsync(long studentGroupId, 
            params DashboardResultType[] type)
        {
            var studentGroupResult = new StudentGroupResultDto();

            var studentGroup = await _unitOfWork.DashboardRepository.GetStudentGroupByIdAsync(studentGroupId);

            if (studentGroup == null)
            {
                return Result<StudentGroupResultDto>.Error(ErrorCode.NotFound, "No student group found");
            }

            studentGroupResult.StudentGroupId = studentGroupId;
            studentGroupResult.CourceId = studentGroup.CourseId;

            var studentsOfStudentGroup = await _unitOfWork.DashboardRepository
                .GetStudentsOfStudentGroup(studentGroupResult.StudentGroupId);

            if (studentsOfStudentGroup != null)
            {
                studentGroupResult.StudentsResultsOfStudentGroup = new List<StudentResultsOfStudentGroupDto>();

                for (int studentNumber = 0; studentNumber < studentsOfStudentGroup.Count; studentNumber++)
                {
                    var studentResultsOfStudentGroupDto = await GetStudentOfGroupResultsAsync(studentsOfStudentGroup[studentNumber].Id, 
                        studentGroupId, type);

                    if (studentResultsOfStudentGroupDto.Data == null)
                    { 
                    studentGroupResult.StudentsResultsOfStudentGroup.Add(studentResultsOfStudentGroupDto.Data);
                    }
                }
            }

            return Result<StudentGroupResultDto>.Success(studentGroupResult);
        }

        public async Task<Result<StudentResultsOfStudentGroupDto>> GetStudentOfGroupResultsAsync(long studentId, 
            long studentGroupId, params DashboardResultType[] type)
        {
            var studentResultsOfStudentGroupDto = new StudentResultsOfStudentGroupDto()
            {
                StudentId = studentId
            };

            if (type.Contains(DashboardResultType.AverageStudentMark))
            {
                var studentAverageMark = await GetStudentOfGroupAverageMarkAsync(studentId, studentGroupId);

                studentResultsOfStudentGroupDto.StudentAverageMark = studentAverageMark.Data ?? null;
            }

            if (type.Contains(DashboardResultType.AverageStudentVisits))
            {
                var studentAverageGroupVisits = await GetStudentAverageVisitsResultsAsync(studentId, studentGroupId);

                studentResultsOfStudentGroupDto.StudentVisitsPercentage = studentAverageGroupVisits.Data ?? null;
            }

            if (type.Contains(DashboardResultType.Classbook))
            {
                var studentLessonsResults = await GetStudentClassBookResultsAsync(studentId, studentGroupId);

                studentResultsOfStudentGroupDto.LessonsResultDto = studentLessonsResults.Data ?? null;
            }

            return Result<StudentResultsOfStudentGroupDto>.Success(studentResultsOfStudentGroupDto);
        }

        public async Task<Result<double?>> GetStudentOfGroupAverageMarkAsync(long studentId, long studentGroupId)
        {
            var lessons = await _unitOfWork.DashboardRepository
                    .GetStudentGroupLessonsAsync(studentGroupId);

            var marksList = (lessons != null) ? (new List<double>()) : null;

            for (int lessonNumber = 0; lessonNumber < lessons.Count; lessonNumber++)
            {
                var studentLessonVisit = await _unitOfWork.DashboardRepository
                    .GetStudentVisitByLessonIdAndStudentId(studentId, lessons[lessonNumber].Id);

                if (studentLessonVisit != null && studentLessonVisit.StudentMark != null)
                {
                    marksList.Add((double)studentLessonVisit.StudentMark);
                }
            }

            if (marksList.Count > 0)
            {
                return Result<double?>.Success(marksList.Average());
            }

            return Result<double?>.Error(ErrorCode.NotFound, "No marks of student found"); ;
        }

        public async Task<Result<int?>> GetStudentAverageVisitsResultsAsync(long studentId, long studentGroupId)
        {
            var totalVisits = await _unitOfWork.DashboardRepository
                                    .GetStudentVisitsByStudentIdAndStudGroupAsync(studentGroupId,
                                    studentId);

            if (totalVisits != null && totalVisits.Count != 0)
            {
                var visitsList = new List<bool>();

            for (int visitNumber = 0; visitNumber < totalVisits.Count; visitNumber++)
            {
                    if (totalVisits[visitNumber].Presence == true)
                    { 
                        visitsList.Add(totalVisits[visitNumber].Presence);
                    }
                }

            int averageVisitPercentage = (int)((double)visitsList.Count / (double)totalVisits.Count * 100);

            return Result<int?>.Success(averageVisitPercentage);
            }

            return Result<int?>.Error(ErrorCode.NotFound, "No student visit data found");
        }

        public async Task<Result<IList<LessonResultDto>>> GetStudentClassBookResultsAsync(long studentId, long studentGroupId)
        {
            var visitsOfSelectedStudent = await _unitOfWork.DashboardRepository
                                .GetStudentVisitsByStudentIdAndStudGroupAsync(studentGroupId,
                                studentId);

            if (visitsOfSelectedStudent != null)
            {
                var lessonsVisitsList = new List<LessonResultDto>();

                for (int visitNumber = 0; visitNumber < visitsOfSelectedStudent.Count; visitNumber++)
                {
                    var lessonResultDto = new LessonResultDto()
                    {
                        LessonId = visitsOfSelectedStudent[visitNumber].LessonId,
                        LessonDate = visitsOfSelectedStudent[visitNumber].Lesson.LessonDate,
                        Presence = visitsOfSelectedStudent[visitNumber].Presence,
                        StudentMark = visitsOfSelectedStudent[visitNumber].StudentMark,
                        Comment = visitsOfSelectedStudent[visitNumber].Comment
                    };

                    lessonsVisitsList.Add(lessonResultDto);
                }

                return Result<IList<LessonResultDto>>.Success(lessonsVisitsList);
            }

            return Result<IList<LessonResultDto>>.Error(ErrorCode.NotFound, "No student classbook data found");
        }

    }
    }

