using CharlieBackend.Core;
using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Exceptions;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class LessonRepository : Repository<Lesson>, ILessonRepository
    {
        public LessonRepository(ApplicationContext applicationContext)
            : base(applicationContext)
        {
        }

        public override Task<List<Lesson>> GetAllAsync()
        {
            return _applicationContext.Lessons
                    .Include(lesson => lesson.Visits)
                    .Include(lesson => lesson.Theme)
                    .ToListAsync();
        }

        public Task<Lesson> GetLastLesson()
        {
            return _applicationContext.Lessons.OrderByDescending(l => l.LessonDate).FirstOrDefaultAsync();
        }

        public Task<List<Lesson>> GetLessonsByDate(DateTime? startDate, DateTime? finishDate)
        {
            return _applicationContext.Lessons.Include(lesson => lesson.Theme)
                  .WhereIf(startDate != null && startDate != default(DateTime),
                   x => x.LessonDate >= startDate)
                  .WhereIf(finishDate != null && finishDate != default(DateTime),
                   x => x.LessonDate <= finishDate)
                  .Include(x => x.Visits)
                  .ToListAsync();
        }

        public Task<List<Lesson>> GetAllLessonsForMentor(long mentorId)
        {
            return _applicationContext.Lessons
                .Where(lesson => lesson.MentorId == mentorId)
                .Select(lesson => lesson)
                .Include(lesson => lesson.Theme)
                .ToListAsync();
        }

        public Task<List<Lesson>> GetAllLessonsForStudentGroup(long studentGroupId)
        {
            return _applicationContext.Lessons
                .Where(lesson => lesson.StudentGroupId == studentGroupId)
                .ToListAsync();
        }

        public async Task<Lesson> GetLessonByHomeworkId(long homeworkId)
        {
            var homework = await _applicationContext.Homeworks.FirstOrDefaultAsync(x => x.Id == homeworkId);

            return await _applicationContext.Lessons
                .FirstOrDefaultAsync(l => l.Id == homework.LessonId);
        }

        public Task<List<Lesson>> GetLessonsForMentorAsync(long? studentGroupId, DateTime? startDate, DateTime? finishDate, long mentorId)
        {
            return _applicationContext.Lessons
                .Where(x => x.MentorId == mentorId)
                .WhereIf(studentGroupId != default, x => x.StudentGroupId == studentGroupId)
                .WhereIf((startDate != default) && (finishDate != default), x => (startDate < x.LessonDate) && (x.LessonDate < finishDate))
                .ToListAsync();
        }

        public Task<List<Lesson>> GetLessonsForStudentAsync(long? studentGroupId, DateTime? startDate, DateTime? finishDate, long studentId)
        {
            return _applicationContext.StudentsOfStudentGroups
                .Where(
                    studentsOfGroup => studentsOfGroup.StudentId == studentId &&
                    studentsOfGroup.StudentGroupId == studentGroupId
                )
                .Join(
                    _applicationContext.Lessons,
                    studentOfGroup => studentOfGroup.StudentGroupId,
                    l => l.StudentGroupId,
                    (students_of_student_group, lesson) => lesson
                )
                .Where(lesson => (lesson.LessonDate > startDate) && (lesson.LessonDate < finishDate))
                .Include(lesson => lesson.Theme)
                .ToListAsync();
        }

        public override Task<Lesson> GetByIdAsync(long id)
        {
            return _applicationContext.Lessons
                .Include(x => x.Visits)
                .Include(x => x.Theme)
                .FirstOrDefaultAsync(entity => entity.Id == id);
        }

        public async Task<Visit> GetVisitByStudentHomeworkIdAsync(long studentHomeworkId)
        {
            Visit visit = null;

            try
            {
                visit = await _applicationContext.Visits.
                    FirstAsync(visit => visit.Lesson.Homeworks
                    .Any(hw => hw.HomeworkStudents
                    .Any(hws => hws.Id == studentHomeworkId)));
            }
            catch
            {
                throw new NotFoundException($"Student howework with id {studentHomeworkId} not found");
            }

            return visit;
        }
        public Task<bool> DoesLessonWithThemeExist(long themeId)
            => _applicationContext.Lessons.AnyAsync(l => l.ThemeId == themeId);
    }
}
