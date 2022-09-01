using CharlieBackend.Core;
using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                    Course = x.Lesson.StudentGroup.Course.Name,
                    StudentGroup = x.Lesson.StudentGroup.Name,
                    Student = $"{x.Student.Account.FirstName} {x.Student.Account.LastName}",
                    Presence = x.Presence,
                }).ToListAsync();

            var studentsAverageVisitsList = studentsVisitsList
                .GroupBy(x => new
                {
                    Student = x.Student,
                    Group = x.StudentGroup,
                    Course = x.Course
                })
                .Select(x => new AverageStudentVisitsDto
                {
                    Course = x.Key.Course,
                    StudentGroup = x.Key.Group,
                    Student = x.Key.Student,
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
                        Course = x.Lesson.StudentGroup.Course.Name,
                        StudentGroup = x.Lesson.StudentGroup.Name,
                        Student = $"{x.Student.Account.FirstName} {x.Student.Account.LastName}",
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
            var visitsList = await _applicationContext.Visits
                    .AsNoTracking()
                    .Where(x => studentGroupIds.Contains((long)x.Lesson.StudentGroupId))
                    .WhereIf(startDate != null && startDate != default(DateTime), x => x.Lesson.LessonDate >= startDate)
                    .WhereIf(finishDate != null && finishDate != default(DateTime), x => x.Lesson.LessonDate <= finishDate)
                    .Select(x => new StudentVisitDto
                    {
                        Course = x.Lesson.StudentGroup.Course.Name,
                        StudentGroup = x.Lesson.StudentGroup.Name,
                        Student = $"{x.Student.Account.FirstName} {x.Student.Account.LastName}",
                        StudentId = x.Student.AccountId,
                        LessonId = x.LessonId,
                        LessonDate = x.Lesson.LessonDate,
                        Presence = x.Presence,
                    })
                    .ToListAsync();

            return visitsList;
        }

        public async Task<List<StudentMarkDto>> GetStudentsMarksListByGroupIdsAndDate(IEnumerable<long> studentGroupIds,
            DateTime? startDate, DateTime? finishDate)
        {
            var visitsList = await _applicationContext.Visits
                    .AsNoTracking()
                    .Where(x => studentGroupIds.Contains((long)x.Lesson.StudentGroupId))
                    .WhereIf(startDate != null && startDate != default(DateTime), x => x.Lesson.LessonDate >= startDate)
                    .WhereIf(finishDate != null && finishDate != default(DateTime), x => x.Lesson.LessonDate <= finishDate)
                    .Select(x => new StudentMarkDto
                    {
                        Course = x.Lesson.StudentGroup.Course.Name,
                        StudentGroup = x.Lesson.StudentGroup.Name,
                        Student = $"{x.Student.Account.FirstName} {x.Student.Account.LastName}",
                        StudentId = x.Student.AccountId,
                        LessonId = x.LessonId,
                        LessonDate = x.Lesson.LessonDate,
                        MarkId = x.Mark.Id,
                        Comment = x.Mark.Comment,
                        Mark = x.Mark.Value
                    })
                    .ToListAsync();

            return visitsList;
        }

        public async Task<List<StudentMarkDto>> GetStudentsMarksListByStudentIds(IEnumerable<long> studentIds)
        {
            var studentsMarksList = await _applicationContext.Visits
                    .AsNoTracking()
                    .Where(x => (x.StudentId != default) &&
                                studentIds.ToList().Contains((long)x.StudentId))
                    .Select(x => new StudentMarkDto
                    {
                        Course = x.Lesson.StudentGroup.Course.Name,
                        StudentGroup = x.Lesson.StudentGroup.Name,
                        Student = $"{x.Student.Account.FirstName} {x.Student.Account.LastName}",
                        LessonId = x.LessonId,
                        LessonDate = x.Lesson.LessonDate,
                        MarkId = x.MarkId,
                        Mark = x.Mark.Value
                    }).ToListAsync();

            return studentsMarksList;
        }

        public async Task<List<AverageStudentMarkDto>> GetStudentHomeworkAvgMarksAsync(IEnumerable<long> studentIds,
            IEnumerable<long> studentGroupsIds)
        {
            var result = new List<AverageStudentMarkDto>();

            if (studentGroupsIds.Any())
            {
                result = await GetStudentHomeworkAverageMarks(studentIds, studentGroupsIds);
            }

            return result;
        }

        public async Task<List<AverageStudentMarkDto>> GetStudentAvgMarksAsync(IEnumerable<long> studentIds,
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

        private async Task<List<AverageStudentMarkDto>> GetStudentHomeworkAverageMarks(IEnumerable<long> studentIds,
    IEnumerable<long> studentGroupsIds)
        {
            return await _applicationContext.HomeworkStudents
                  .AsNoTracking()
                  .Where(x => studentGroupsIds.Contains(x.Homework.Lesson.StudentGroupId.Value))
                  .Where(x => studentIds.Contains((long)x.StudentId))
                  .Where(x => x.Mark != null)
                  .Select(x => new
                  {
                      Course = x.Homework.Lesson.StudentGroup.Course.Name,
                      StudentGroup = x.Homework.Lesson.StudentGroup.Name,
                      StudentMark = (decimal)x.Mark.Value,
                      Student = $"{x.Student.Account.FirstName} {x.Student.Account.LastName}"
                  })
                  .GroupBy(x => new
                  {
                      Group = x.StudentGroup,
                      Course = x.Course,
                      Student = x.Student
                  })
                  .Select(x => new AverageStudentMarkDto
                  {
                      Course = x.Key.Course,
                      StudentGroup = x.Key.Group,
                      Student = x.Key.Student,
                      StudentAverageMark = x.Average(s => s.StudentMark)
                  }
                  ).ToListAsync();
        }

        private async Task<List<AverageStudentMarkDto>> GetStudentAverageMarks(IEnumerable<long> studentIds,
            IEnumerable<long> studentGroupsIds)
        {
              return await _applicationContext.Visits
                    .AsNoTracking()
                    .Where(x => studentGroupsIds.Contains(x.Lesson.StudentGroupId.Value))
                    .Where(x => studentIds.Contains((long)x.StudentId))
                    .Where(x => x.MarkId != null)
                    .Select(x => new
                    {
                        Course = x.Lesson.StudentGroup.Course.Name,
                        StudentGroup = x.Lesson.StudentGroup.Name,
                        StudentLessonMark = (decimal)x.Mark.Value,
                        Student = $"{x.Student.Account.FirstName} {x.Student.Account.LastName}"
                    })
                    .GroupBy(x => new
                    {
                        Group = x.StudentGroup,
                        Course = x.Course,
                        Student = x.Student
                    })
                    .Select(x => new AverageStudentMarkDto
                    {
                        Course = x.Key.Course,
                        StudentGroup = x.Key.Group,
                        Student = x.Key.Student,
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
                    Course = x.Lesson.StudentGroup.Course.Name,
                    StudentGroup = x.Lesson.StudentGroup.Name,
                    Student = $"{x.Student.Account.FirstName} {x.Student.Account.LastName}",
                    Presence = x.Presence,
                }).ToListAsync();

            var StudentAverageVisitsPercentage = studentVisitsList
                .GroupBy(x => new
                {
                    Student = x.Student,
                    Group = x.StudentGroup,
                    Course = x.Course
                })
                .Select(x => new AverageStudentVisitsDto
                {
                    Course = x.Key.Course,
                    StudentGroup = x.Key.Group,
                    Student = x.Key.Student,
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
                        Course = x.Lesson.StudentGroup.Course.Name,
                        StudentGroup = x.Lesson.StudentGroup.Name,
                        Student = $"{x.Student.Account.FirstName} {x.Student.Account.LastName}",
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
                .Where(x => x.StudentId == studentId && x.MarkId != null)
                .Select(x => new StudentMarkDto
                {
                    Course = x.Lesson.StudentGroup.Course.Name,
                    StudentGroup = x.Lesson.StudentGroup.Name,
                    Student = $"{x.Student.Account.FirstName} {x.Student.Account.LastName}",
                    LessonId = x.LessonId,
                    LessonDate = x.Lesson.LessonDate,
                    MarkId = x.MarkId,
                    Mark = x.Mark.Value
                }).ToListAsync();

            return studentsMarksList;
        }

        public async Task<List<AverageStudentGroupMarkDto>> GetStudentGroupsAverageMarks(List<long> studentGroupIds)
        {
            var studentGroupMarskList = await _applicationContext.Visits
                .AsNoTracking()
                .Where(x => studentGroupIds.Contains(x.Lesson.StudentGroupId.Value))
                .Where(x => x.MarkId != null)
                .Select(x => new
                {
                    Course = x.Lesson.StudentGroup.Course.Name,
                    StudentGroup = x.Lesson.StudentGroup.Name,
                    StudentMark = (decimal)x.Mark.Value
                })
                .GroupBy(x => new
                {
                    Group = x.StudentGroup,
                    Course = x.Course
                })
                .Select(x => new AverageStudentGroupMarkDto
                {
                    Course = x.Key.Course,
                    StudentGroup = x.Key.Group,
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
                    Course = x.Lesson.StudentGroup.Course.Name,
                    StudentGroup = x.Lesson.StudentGroup.Name,
                    StudentPresense = x.Presence
                })
                .ToListAsync();

            var studentGroupAveragevisits = studentGroupVisits
                .GroupBy(x => new
                {
                    Group = x.StudentGroup,
                    Course = x.Course
                })
                .Select(x => new AverageStudentGroupVisitDto
                {
                    Course = x.Key.Course,
                    StudentGroup = x.Key.Group,
                    AverageVisitPercentage = (int)((double)x
                     .Where(d => d.StudentPresense == true).Count()
                      / (double)x.Count() * 100)
                }).ToList();

            return studentGroupAveragevisits;
        }
    }
}
