﻿using System;
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

        public async Task<List<long>> GetGroupsIdsByCourseIdAsync(long courseId, 
            DateTime? startDate, DateTime? finishDate)
        {
            List<long> groupIdsbyCourseIdAndPeriod = await GetGroupIds(startDate, finishDate, courseId);

            return groupIdsbyCourseIdAndPeriod;
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
                && groupsIds.Contains(x.StudentGroupId.Value)))
                .Select(x => x.Id)
                .ToListAsync();

            return studentsIdsBygroupsIds;
        }

        public async Task<List<AverageStudentVisitsDto>> GetStudentsAverageVisitsByStudentIdsAndGroupsIdsAsync(IEnumerable<long> studentIds, 
            IEnumerable<long> studentGroupIds)
        {
            var studentsVisitsList = await _applicationContext.Visits
                .AsNoTracking()
                .Where(x => x.Lesson.StudentGroup.StudentsOfStudentGroups
                        .Any(x => studentIds.Contains(x.Student.Id)))
                .WhereIf(studentGroupIds != default && studentGroupIds.Any(), 
                        x => studentGroupIds.Contains(x.Lesson.StudentGroupId.Value))
                .Select(x => new StudentVisitDto
                {
                    CourseId = (long)x.Lesson.StudentGroup.CourseId,
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
                    .Where(x => x.Lesson.StudentGroup.StudentsOfStudentGroups
                    .Any(x => studentIds.Contains(x.Student.Id)))
                    .Select(x => new StudentVisitDto
                    {
                        CourseId = (long)x.Lesson.StudentGroup.CourseId,
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
                        CourseId = (long)x.Lesson.StudentGroup.CourseId,
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
                        StudentLessonMark = x.StudentMark,
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
                        CourseId = (long)x.Key.CourseId,
                        StudentGroupId = (long)x.Key.GroupId,
                        StudentId = (long)x.Key.StudentId,
                        StudentAverageMark = Math.Round(x.Sum(x => (double)x.StudentLessonMark)
                        / x.Count(), 2)
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
                    CourseId = (long)x.Lesson.StudentGroup.CourseId,
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
                      / (double)x.Count() * 100)
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
                        CourseId = (long)x.Lesson.StudentGroup.CourseId,
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
                    CourseId = (long)x.Lesson.StudentGroup.CourseId,
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
                .Select( x => new
                {
                    CourseId = x.Lesson.StudentGroup.CourseId,
                    StudentGroupId = (long)x.Lesson.StudentGroupId,
                    StudentMark = x.StudentMark
                })
                .AsQueryable()
                .GroupBy(x => new
                {
                    GroupId = x.StudentGroupId,
                    CourseId = x.CourseId
                })
                .Select(x => new AverageStudentGroupMarkDto
                {
                    CourseId = (long)x.Key.CourseId,
                    StudentGroupId = x.Key.GroupId,
                    AverageMark = Math.Round(x.Average(x => (double)x.StudentMark), 2)
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
                    CourseId = (long)x.Key.CourseId,
                    StudentGroupId = x.Key.GroupId,
                    AverageVisitPercentage = (int)((double)x
                     .Where(d => d.StudentPresense == true).Count()
                      / (double)x.Count() * 100)
                }).ToList();

            return studentGroupAveragevisits;
        }
    }
}
