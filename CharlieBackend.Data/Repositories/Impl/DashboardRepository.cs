using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using CharlieBackend.Core;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class DashboardRepository : Repository<StudentGroup>, IDashboardRepository
    {
        public DashboardRepository(ApplicationContext applicationContext)
                : base(applicationContext)
        {
        }

        public async Task<List<long>> GetGroupsIdsByCourseIdAndPeriodAsync(long courseId,
            DateTime? startDate, DateTime? finishDate)
        {
            List<long> groupIdsbyCourseIdAndPeriod = await GetGroupIds(startDate, finishDate, courseId);

            return groupIdsbyCourseIdAndPeriod;
        }

        public async Task<List<long>> GetGroupsIdsByCourseIdAsync(long courseId)
        {
            return await _applicationContext.StudentGroups
                .AsNoTracking()
                .Where(x => x.CourseId == courseId)
                .Select(x => x.Id)
                .ToListAsync();
        }

        public async Task<List<long>> GetGroupsIdsByStudentIdAndPeriodAsync(long studentId,
            DateTime? startDate, DateTime? finishDate)
        {
            List<long> groupIdsbyStudentIdAndPeriod = await GetGroupIds(startDate, finishDate, studentId: studentId);

            return groupIdsbyStudentIdAndPeriod;
        }

        private async Task<List<long>> GetGroupIds(DateTime? startDate, DateTime? finishDate, long? courseId = null, long? studentId = null)
        {
            return await _applicationContext.StudentGroups
                .AsNoTracking()
                .WhereIf(courseId != default, x => x.CourseId == courseId)
                .WhereIf(studentId != default, x => x.StudentsOfStudentGroups.Any(x => x.StudentId == studentId))
                .WhereIf(startDate != null && startDate != default(DateTime),
                x => x.StartDate >= startDate)
                .WhereIf(finishDate != null && finishDate != default(DateTime),
                x => x.FinishDate <= finishDate)
                .Select(x => x.Id)
                .ToListAsync();
        }

        public async Task<List<long>> GetStudentsIdsByGroupIdsAsync(IEnumerable<long> groupsIds)
        {
            var studentsIdsBygroupsIds = await _applicationContext.Students
                .AsNoTracking()
                .Where(x => x.StudentsOfStudentGroups.Any(x => x.StudentGroupId.HasValue
                && groupsIds.ToList().Contains(x.StudentGroupId.Value)))
                .Select(x => x.Id)
                .ToListAsync();

            return studentsIdsBygroupsIds;
        }

        public async Task<List<AverageStudentVisitsDto>> GetStudentsAverageVisitsByStudentIdsAndGroupsIdsAsync(IEnumerable<long> studentIds,
            IEnumerable<long> studentGroupIds)
        {
            var studentsVisitsList = await _applicationContext.Visits
                .AsNoTracking()
                .Where(x => studentIds.ToList().Contains((long)x.StudentId))
                .WhereIf(studentGroupIds != default &&
                         studentGroupIds.Any(),
                         x => (x.Lesson.StudentGroupId != null) &&
                         studentGroupIds.ToList().Contains((long)x.Lesson.StudentGroupId))
                .Select(x => new StudentVisitDto
                {
                    CourseId = x.Lesson.StudentGroup.CourseId,
                    StudentGroupId = (long)x.Lesson.StudentGroupId,
                    StudentId = (long)x.StudentId,
                    Presence = x.Presence,
                }).ToListAsync();

            var studentsAverageVisitsList = studentsVisitsList
                .GroupBy(x => new
                {
                    StudentId = x.StudentId,
                    GroupId = x.StudentGroupId,
                    CourseId = x.CourseId
                })
                .Select(x => new AverageStudentVisitsDto
                {
                    CourseId = x.Key.CourseId,
                    StudentGroupId = x.Key.GroupId,
                    StudentId = x.Key.StudentId,
                    StudentAverageVisitsPercentage = (int)((double)x
                     .Where(d => d.Presence == true).Count()
                      / (double)x.Count() * 100)
                }).ToList();

            return studentsAverageVisitsList;
        }

        public async Task<List<StudentVisitDto>> GetStudentsPresenceListByStudentIds(IEnumerable<long> studentIds)
        {
            var studentsPresenceList = await _applicationContext.Visits
                    .AsNoTracking()
                    .Where(x => studentIds.ToList().Contains((long)x.StudentId))
                    .Select(x => new StudentVisitDto
                    {
                        CourseId = x.Lesson.StudentGroup.CourseId,
                        StudentGroupId = (long)x.Lesson.StudentGroupId,
                        StudentId = (long)x.StudentId,
                        LessonId = x.LessonId,
                        LessonDate = x.Lesson.LessonDate,
                        Presence = x.Presence,
                    })
                    .ToListAsync();

            return studentsPresenceList;
        }

        public async Task<List<StudentVisitDto>> GetStudentsPresenceListByGroupIdsAndDate(IEnumerable<long> studentGroupIds,
            DateTime? startDate, DateTime? finishDate)
        {
            return await _applicationContext.Visits
                    .AsNoTracking()
                    .Where(x => studentGroupIds.Contains((long)x.Lesson.StudentGroupId))
                    .WhereIf(startDate != null && startDate != default(DateTime), x => x.Lesson.LessonDate >= startDate)
                    .WhereIf(finishDate != null && finishDate != default(DateTime), x => x.Lesson.LessonDate <= finishDate)
                    .Select(x => new StudentVisitDto
                    {
                        CourseId = x.Lesson.StudentGroup.CourseId,
                        StudentGroupId = (long)x.Lesson.StudentGroupId,
                        StudentId = (long)x.StudentId,
                        LessonId = x.LessonId,
                        LessonDate = x.Lesson.LessonDate,
                        Presence = x.Presence,
                    })
                    .ToListAsync();
        }

        public async Task<List<StudentMarkDto>> GetStudentsMarksListByGroupIdsAndDate(IEnumerable<long> studentGroupIds,
            DateTime? startDate, DateTime? finishDate)
        {
            return await _applicationContext.Visits
                    .AsNoTracking()
                    .Where(x => studentGroupIds.Contains((long)x.Lesson.StudentGroupId))
                    .WhereIf(startDate != null && startDate != default(DateTime),
                            x => x.Lesson.LessonDate >= startDate)
                    .WhereIf(finishDate != null && finishDate != default(DateTime),
                            x => x.Lesson.LessonDate <= finishDate)
                    .Select(x => new StudentMarkDto
                    {
                        CourseId = x.Lesson.StudentGroup.CourseId,
                        StudentGroupId = (long)x.Lesson.StudentGroupId,
                        StudentId = (long)x.StudentId,
                        LessonId = x.LessonId,
                        LessonDate = x.Lesson.LessonDate,
                        StudentMark = x.StudentMark,
                    }).ToListAsync();
        }

        public async Task<List<StudentMarkDto>> GetStudentsMarksListByStudentIds(IEnumerable<long> studentIds)
        {
            var studentsMarksList = await _applicationContext.Visits
                    .AsNoTracking()
                    .Where(x => (x.StudentId != default) &&
                                studentIds.ToList().Contains((long)x.StudentId))
                    .Select(x => new StudentMarkDto
                    {
                        CourseId = x.Lesson.StudentGroup.CourseId,
                        StudentGroupId = (long)x.Lesson.StudentGroupId,
                        StudentId = (long)x.StudentId,
                        LessonId = x.LessonId,
                        LessonDate = x.Lesson.LessonDate,
                        StudentMark = x.StudentMark,
                    }).ToListAsync();

            return studentsMarksList;
        }

        public async Task<List<AverageStudentMarkDto>> GetStudentAverageMarksByStudentIdsAndGropsIdsAsync(IEnumerable<long> studentIds,
            IEnumerable<long> studentGroupsIds)
        {
            if (!studentGroupsIds.Any())
            {
                return new List<AverageStudentMarkDto>();
            }
            else
            {
                var studentAverageMarskList = await GetStudentAverageMarks(studentIds, studentGroupsIds);

                return studentAverageMarskList;
            }
        }

        private async Task<List<AverageStudentMarkDto>> GetStudentAverageMarks(IEnumerable<long> studentIds,
            IEnumerable<long> studentGroupsIds)
        {
              return await _applicationContext.Visits
                    .AsNoTracking()
                    .Where(x => studentGroupsIds.Contains(x.Lesson.StudentGroupId.Value))
                    .Where(x => studentIds.Contains((long)x.StudentId))
                    .Where(x => x.StudentMark != null)
                    .Select(x => new
                    {
                        CourseId = x.Lesson.StudentGroup.CourseId,
                        StudentGroupId = x.Lesson.StudentGroupId,
                        StudentLessonMark = (decimal)x.StudentMark,
                        StudentId = x.StudentId
                    })
                    .AsQueryable()
                    .GroupBy(x => new
                    {
                        GroupId = x.StudentGroupId,
                        CourseId = x.CourseId,
                        StudentId = x.StudentId
                    })
                    .Select(x => new AverageStudentMarkDto
                    {
                        CourseId = x.Key.CourseId,
                        StudentGroupId = (long)x.Key.GroupId,
                        StudentId = (long)x.Key.StudentId,
                        StudentAverageMark = x.Average(s => s.StudentLessonMark)
                    }
                    ).ToListAsync();
        }

        public async Task<List<AverageStudentVisitsDto>> GetStudentAverageVisitsPercentageByStudentIdsAsync(long studentId, List<long> studentGroupsIds)
        {
            var studentVisitsList = await _applicationContext.Visits
                .AsNoTracking()
                .Where(x => studentGroupsIds.Contains(x.Lesson.StudentGroupId.Value))
                .Where(x => x.StudentId == studentId)
                .Select(x => new StudentVisitDto
                {
                    CourseId = x.Lesson.StudentGroup.CourseId,
                    StudentGroupId = (long)x.Lesson.StudentGroupId,
                    StudentId = (long)x.StudentId,
                    Presence = x.Presence,
                }).ToListAsync();

            var StudentAverageVisitsPercentage = studentVisitsList
                .GroupBy(x => new
                {
                    StudentId = x.StudentId,
                    GroupId = x.StudentGroupId,
                    CourseId = x.CourseId
                })
                .Select(x => new AverageStudentVisitsDto
                {
                    CourseId = x.Key.CourseId,
                    StudentGroupId = x.Key.GroupId,
                    StudentId = x.Key.StudentId,
                    StudentAverageVisitsPercentage = (int)((double)x
                     .Where(d => d.Presence == true).Count()
                      / x.Count() * 100)
                }).ToList();

            return StudentAverageVisitsPercentage;
        }

        public async Task<List<StudentVisitDto>> GetStudentPresenceListByStudentIds(long studentId, List<long> studentGroupsIds)
        {
            var studentsPresenceList = await _applicationContext.Visits
                    .AsNoTracking()
                    .Where(x => studentGroupsIds.Contains(x.Lesson.StudentGroupId.Value))
                    .Where(x => x.StudentId == studentId)
                    .Select(x => new StudentVisitDto
                    {
                        CourseId = x.Lesson.StudentGroup.CourseId,
                        StudentGroupId = (long)x.Lesson.StudentGroupId,
                        StudentId = (long)x.StudentId,
                        LessonId = x.LessonId,
                        LessonDate = x.Lesson.LessonDate,
                        Presence = x.Presence,
                    }).ToListAsync();

            return studentsPresenceList;
        }

        public async Task<List<StudentMarkDto>> GetStudentMarksListByStudentIds(long studentId, List<long> studentGroupsIds)
        {
            var studentsMarksList = await _applicationContext.Visits
                .AsNoTracking()
                .Where(x => studentGroupsIds.Contains(x.Lesson.StudentGroupId.Value))
                .Where(x => x.StudentId == studentId && x.StudentMark != null)
                .Select(x => new StudentMarkDto
                {
                    CourseId = x.Lesson.StudentGroup.CourseId,
                    StudentGroupId = (long)x.Lesson.StudentGroupId,
                    StudentId = (long)x.StudentId,
                    LessonId = x.LessonId,
                    LessonDate = x.Lesson.LessonDate,
                    StudentMark = x.StudentMark,
                }).ToListAsync();

            return studentsMarksList;
        }

        public async Task<List<AverageStudentGroupMarkDto>> GetStudentGroupsAverageMarks(List<long> studentGroupIds)
        {
            var studentGroupMarskList = await _applicationContext.Visits
                .AsNoTracking()
                .Where(x => studentGroupIds.Contains(x.Lesson.StudentGroupId.Value))
                .Where(x => x.StudentMark != null)
                .Select(x => new
                {
                    CourseId = x.Lesson.StudentGroup.CourseId,
                    StudentGroupId = (long)x.Lesson.StudentGroupId,
                    StudentMark = (decimal)x.StudentMark
                })
                .AsQueryable()
                .GroupBy(x => new
                {
                    GroupId = x.StudentGroupId,
                    CourseId = x.CourseId
                })
                .Select(x => new AverageStudentGroupMarkDto
                {
                    CourseId = x.Key.CourseId,
                    StudentGroupId = x.Key.GroupId,
                    AverageMark = x.Average(x => x.StudentMark)
                }).ToListAsync();

            return studentGroupMarskList;
        }

        public async Task<List<AverageStudentGroupVisitDto>> GetStudentGroupsAverageVisits(List<long> studentGroupIds)
        {
            var studentGroupVisits = await _applicationContext.Visits
                .AsNoTracking()
                .Where(x => studentGroupIds.Contains(x.Lesson.StudentGroupId.Value))
                .Select(x => new
                {
                    CourseId = x.Lesson.StudentGroup.CourseId,
                    StudentGroupId = (long)x.Lesson.StudentGroupId,
                    StudentPresense = x.Presence
                })
                .ToListAsync();

            var studentGroupAveragevisits = studentGroupVisits
                .GroupBy(x => new
                {
                    GroupId = x.StudentGroupId,
                    CourseId = x.CourseId
                })
                .Select(x => new AverageStudentGroupVisitDto
                {
                    CourseId = x.Key.CourseId,
                    StudentGroupId = x.Key.GroupId,
                    AverageVisitPercentage = (int)((double)x
                     .Where(d => d.StudentPresense == true).Count()
                      / (double)x.Count() * 100)
                }).ToList();

            return studentGroupAveragevisits;
        }
    }
}
