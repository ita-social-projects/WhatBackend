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

        public async Task<List<StudentGroup>> GetAllStudentGroupsAsync()
        {
            return await _applicationContext.StudentGroups.ToListAsync();
        }

        public async Task<StudentGroup> GetStudentGroupByIdAsync(long studentGroupId)
        {
            return await _applicationContext.StudentGroups
                .FirstOrDefaultAsync(x => x.Id == studentGroupId);
        }

        public async Task<List<Lesson>> GetStudentGroupLessonsAsync(long? studentGroupId)
        {
            var lessons = await _applicationContext.Lessons
                    .Include(lesson => lesson.Visits)
                    .Where(lesson => lesson.StudentGroupId == studentGroupId).ToListAsync();

            return lessons;
        }

        public async Task<List<StudentOfStudentGroup>> GetStudentsOfStudentGroup(long studentGroupId)
        {
            var students = await _applicationContext.StudentsOfStudentGroups
                    .Where(studentGroup => studentGroup.StudentGroupId == studentGroupId).ToListAsync();

            return students;
        }

        public async Task<List<Visit>> GetStudentGroupLessonVisitsAsync(long lessonId)
        {
            var lesson = await _applicationContext.Visits
                    .Where(lesson => lesson.LessonId == lessonId).ToListAsync();

            return lesson;
        }

        public async Task<List<Visit>> GetStudentVisitsAsync(long lessonId)
        {
            var lesson = await _applicationContext.Visits
                    .Where(lesson => lesson.LessonId == lessonId).ToListAsync();

            return lesson;
        }

        public async Task<List<Visit>> GetStudentVisitsByStudentIdAndStudGroupAsync(long studentGroupId, long studentId)
        {
            var lessons = await _applicationContext.Visits
                    .Include(visits => visits.Lesson)
                    .Where(studId => studId.StudentId == studentId)
                    .Where(studGroup => studGroup.Lesson.StudentGroupId == studentGroupId)
                    .ToListAsync();

            return lessons;
        }

        public async Task<List<StudentOfStudentGroup>> GetStudentGroupsAsync(long studentId)
        {
            var lessons = await _applicationContext.StudentsOfStudentGroups
                    .Where(studId => studId.StudentId == studentId).ToListAsync();

            return lessons;
        }

        public async Task<Visit> GetStudentVisitByLessonIdAndStudentId(long studentId, long lessonId)
        {
            var visit = await _applicationContext.Visits
                .Where(x => x.StudentId == studentId)
                .FirstOrDefaultAsync(x => x.LessonId == lessonId);

            return visit;
        }

        public async Task<List<StudentGroup>> GetStudentGroupsByCourceIdAsync(long courceId)
        {
            var studentGroups = await _applicationContext.StudentGroups
                .Where(x => x.CourseId == courceId).ToListAsync();

            return studentGroups;
        }


    }
}
