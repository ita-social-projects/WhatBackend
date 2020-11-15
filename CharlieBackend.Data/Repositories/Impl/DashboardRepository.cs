using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class DashboardRepository : Repository<StudentGroup>, IDashboardRepository
    {

        public DashboardRepository(ApplicationContext applicationContext)
                : base(applicationContext)
        {

        }

        public async Task<List<long>> GetGroupsIdsByCourceIdAsync(long courceId, DateTime startDate)
        {
            var groupIdsbyCourceid = await _applicationContext.StudentGroups
                .AsNoTracking()
                .Where(x => x.CourseId == courceId)
                .Where(x => x.StartDate >= startDate)
                .Select(x => x.Id)
                .ToListAsync();

            return groupIdsbyCourceid;
        }

        public async Task<List<long>> GetStudentsIdsByGroupIdsAsync(IEnumerable<long> groupsIds)
        {
            var studentsIdsBygroupsIds = await _applicationContext.Students
                .AsNoTracking()
                .Where(x => x.StudentsOfStudentGroups.Any(x => x.StudentGroupId.HasValue 
                && groupsIds.Contains(x.StudentGroupId.Value)))
                .Select(x => x.Id)
                .ToListAsync();

            return studentsIdsBygroupsIds;
        }

        public async Task<List<long>> GetStudentsIdsByGroupIdAsync(long studentGroupId)
        {
            var studentIdsByStudentGroupId = await _applicationContext.Students
                .AsNoTracking()
                .Where(x => x.StudentsOfStudentGroups
                .Any(x => x.StudentGroupId.HasValue
                && x.StudentGroupId == studentGroupId))
                .Select(x => x.Id)
                .ToListAsync();

            return studentIdsByStudentGroupId;
        }

        public async Task<List<AverageStudentMarkDto>> GetStudentsAverageMarksByStudentIdsAsync(IEnumerable<long> studentIds)
        {
            var studentsAverageMarks = await _applicationContext.Visits
                    .AsNoTracking()
                    .Where(x => x.Lesson.StudentGroup.StudentsOfStudentGroups
                    .Any(x => studentIds.Contains(x.Student.Id))
                    && x.StudentMark != null)
                    .Select(x => new
                    {
                        CourceId = x.Lesson.StudentGroup.CourseId,
                        StudentGroupId = x.Lesson.StudentGroupId,
                        StudentId = x.StudentId,
                        StudentLessonMark = x.StudentMark,
                    }).AsQueryable()
                    .GroupBy(x => new
                    {
                        StudentId = x.StudentId,
                        GroupId = x.StudentGroupId,
                        CourceId = x.CourceId
                    })
                    .Select(x => new AverageStudentMarkDto
                    {
                        CourceId = x.Key.CourceId,
                        StudentGroupId = (long)x.Key.GroupId,
                        StudentId = (long)x.Key.StudentId,
                        StudentAverageMark = x.Sum(x => (double)x.StudentLessonMark) / x.Count()
                    }
                    ).ToListAsync();

            return studentsAverageMarks;
        }

        public async Task<List<AverageStudentVisitsDto>> GetStudentsAverageVisitsByStudentIdsAsync(IEnumerable<long> studentIds)
        {
            var studentsVisitsList = await _applicationContext.Visits
                .AsNoTracking()
                .Where(x => x.Lesson.StudentGroup.StudentsOfStudentGroups
                .Any(x => studentIds.Contains(x.Student.Id)))
                .Select(x => new StudentVisitDto
                {
                    CourceId = x.Lesson.StudentGroup.CourseId,
                    StudentGroupId = (long)x.Lesson.StudentGroupId,
                    StudentId = (long)x.StudentId,
                    Presence = x.Presence,
                }).ToListAsync();

            var studentsAverageVisitsList = studentsVisitsList
                .GroupBy(x => new
                {
                    StudentId = x.StudentId,
                    GroupId = x.StudentGroupId,
                    CourceId = x.CourceId
                })
                .Select(x => new AverageStudentVisitsDto
                {
                     CourceId = x.Key.CourceId,
                     StudentGroupId = x.Key.GroupId,
                     StudentId = x.Key.StudentId,
                     StudentAverageVisits = (int)((double)x
                     .Where(d => d.Presence == true).Count()
                      / (double)x.Count() * 100)
                 }).ToList();
                

            return studentsAverageVisitsList;
        }

        public List<AverageStudentVisitsDto> GetStudentsAverageVisitsByStudentsVisits(List<StudentVisitDto> studentsVisits)
        {
            var studentsAverageVisits = studentsVisits
                    .GroupBy(x => new
                    {
                        StudentId = x.StudentId,
                        GroupId = x.StudentGroupId,
                        CourceId = x.CourceId
                    })
                    .Select(x => new AverageStudentVisitsDto
                    {
                        CourceId = x.Key.CourceId,
                        StudentId = x.Key.StudentId,
                        StudentAverageVisits = (int)((double)x
                            .Where(d => d.Presence == true).Count()
                            / (double)x.Count() * 100)
                    }
                    ).ToList();

            return studentsAverageVisits;
        }

        public async Task<List<StudentVisitDto>> GetStudentsPresenceListByStudentIds(IEnumerable<long> studentIds)
        {
            var studentsPresenceList = await _applicationContext.Visits
                    .AsNoTracking()
                    .Where(x => x.Lesson.StudentGroup.StudentsOfStudentGroups
                    .Any(x => studentIds.Contains(x.Student.Id)))
                    .Select(x => new StudentVisitDto
                    {
                        CourceId = x.Lesson.StudentGroup.CourseId,
                        StudentGroupId = (long)x.Lesson.StudentGroupId,
                        StudentId = (long)x.StudentId,
                        LessonId = x.LessonId,
                        LessonDate = x.Lesson.LessonDate,
                        Presence = x.Presence,
                    }).ToListAsync();

            return studentsPresenceList;
        }

        public async Task<List<StudentMarkDto>> GetStudentsMarksListByStudentIds(IEnumerable<long> studentIds)
        {
            var studentsMarksList = await _applicationContext.Visits
                    .AsNoTracking()
                    .Where(x => x.Lesson.StudentGroup.StudentsOfStudentGroups
                    .Any(x => studentIds.Contains(x.Student.Id)) 
                    && x.StudentMark != null)
                    .Select(x => new StudentMarkDto
                    {
                        CourceId = x.Lesson.StudentGroup.CourseId,
                        StudentGroupId = (long)x.Lesson.StudentGroupId,
                        StudentId = (long)x.StudentId,
                        LessonId = x.LessonId,
                        LessonDate = x.Lesson.LessonDate,
                        StudentMark = x.StudentMark,
                    }).ToListAsync();

            return studentsMarksList;
        }

    }
}
