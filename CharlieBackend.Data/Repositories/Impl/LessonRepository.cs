using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using CharlieBackend.Core;
using System;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class LessonRepository : Repository<Lesson>, ILessonRepository
    {
        public LessonRepository(ApplicationContext applicationContext) 
            : base(applicationContext) 
        { 
        }

        public new Task<List<Lesson>> GetAllAsync()
        {
            return _applicationContext.Lessons
                    .Include(lesson => lesson.Visits)
                    .Include(lesson => lesson.Theme)
                    .ToListAsync();
        }

        public async Task<List<Lesson>> GetAllLessonsForMentor(long mentorId)
        {
            return await _applicationContext.Lessons
                .Where(lesson => lesson.MentorId == mentorId)
                .Select(lesson => lesson)
                .ToListAsync();
        }

        public async Task<List<Lesson>> GetLessonsForMentorAsync(long? studentGroupId, DateTime? startDate, DateTime? finishDate, long mentorId)
        {
            return await _applicationContext.Lessons
                .Where(x=> x.MentorId == mentorId)
                .WhereIf(studentGroupId != default, x => x.StudentGroupId == studentGroupId)
                .WhereIf((startDate != default) && (finishDate != default), x => (startDate < x.LessonDate) && (x.LessonDate < finishDate))
                .ToListAsync();
        }

        public async Task<List<Lesson>> GetLessonsForStudentAsync(long? studentGroupId, DateTime? startDate, DateTime? finishDate, long studentId)
        {
            return await _applicationContext.StudentsOfStudentGroups
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
                .ToListAsync();
        }

        public async Task<IList<StudentLessonDto>> GetStudentInfoAsync(long studentId)
        {
            try
            {
                var studentLessonDtos = new List<StudentLessonDto>();

                var visits = await _applicationContext.Visits
                        .Include(visit => visit.Lesson)
                        .ThenInclude(lesson => lesson.Theme)
                        .Where(visit => visit.StudentId == studentId).ToListAsync();

                for (int i = 0; i < visits.Count; i++)
                {
                    var studentLessonDto = new StudentLessonDto
                    {
                        Id = visits[i].Lesson.Id,
                        Comment = visits[i].Comment,
                        Mark = visits[i].StudentMark,
                        Presence = visits[i].Presence,
                        ThemeName = visits[i].Lesson.Theme.Name,
                        LessonDate = visits[i].Lesson.LessonDate,
                        StudentGroupId = visits[i].Lesson.StudentGroupId
                    };

                    studentLessonDtos.Add(studentLessonDto);
                }

                return studentLessonDtos;
            }
            catch 
            {
                return null; 
            }
        }
    }
}
