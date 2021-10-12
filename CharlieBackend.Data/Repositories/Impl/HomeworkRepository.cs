using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Data.Helpers;
using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using CharlieBackend.Core.DTO.Homework;
using CharlieBackend.Core;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class HomeworkRepository : Repository<Homework>, IHomeworkRepository
    {
        public HomeworkRepository(ApplicationContext applicationContext)
        : base(applicationContext)
        {
        }

        public async override Task<Homework> GetByIdAsync(long homeworkId)
        {
            return await _applicationContext.Homeworks
                        .Include(x => x.Lesson)
                        .Include(x => x.AttachmentsOfHomework)
                        .FirstOrDefaultAsync(x => x.Id == homeworkId);
        }

        public void UpdateManyToMany(IEnumerable<AttachmentOfHomework> currentHomeworkAttachments,
                             IEnumerable<AttachmentOfHomework> newHomeworkAttachments)
        {
            _applicationContext.AttachmentsOfHomework.
                    TryUpdateManyToMany(currentHomeworkAttachments, newHomeworkAttachments);
        }

        public async Task<IList<Homework>> GetHomeworksByLessonId(long lessonId)
        {
            return await _applicationContext.Homeworks
                .Include(x => x.Lesson)
                .Include(x => x.AttachmentsOfHomework)
                .Where(x => x.LessonId == lessonId).ToListAsync();
        }

        public async Task<IList<Homework>> GetHomeworks(GetHomeworkRequestDto request)
        {
            return await GetHomeworksBase(request).ToListAsync(); 
        } 

        public async Task<IList<Homework>> GetHomeworksForMentor(GetHomeworkRequestDto request, long mentorId)
        {
            return await GetHomeworksBase(request)
                    .Where(x => x.Lesson.StudentGroup.Course.MentorsOfCourses.Any(moc => moc.MentorId == mentorId))
                    .ToListAsync();
        }

        public async Task<IList<Homework>> GetHomeworksForStudent(GetHomeworkRequestDto request, long studentId)
        {
            return await GetHomeworksBase(request)
                    .Where(x => x.Lesson.StudentGroup.StudentsOfStudentGroups.Any(sg => sg.StudentId == studentId))
                    .ToListAsync();
        }

        //TODO: why x.Lesson.MentorId == mentorId ? Should we grant these rights to all mentors on the course?
        public async Task<Homework> GetMentorHomeworkAsync(long mentorId, long homeworkId)
        {
            return await _applicationContext.Homeworks
                .Include(x => x.Lesson)
                .FirstOrDefaultAsync(x => (x.Id == homeworkId) && (x.Lesson.MentorId == mentorId));
        }

        private IQueryable<Homework> GetHomeworksBase(GetHomeworkRequestDto request) => _applicationContext.Homeworks
                    .Include(x => x.AttachmentsOfHomework)
                    .Include(x => x.Lesson)
                         .ThenInclude(l => l.StudentGroup)
                            .ThenInclude(c => c.Course)
                                .ThenInclude(m => m.MentorsOfCourses)
                    .WhereIf(request.ThemeId.HasValue,
                            h => h.Lesson.ThemeId == request.ThemeId)
                    .WhereIf(request.GroupId.HasValue,
                            l => l.Lesson.StudentGroupId == request.GroupId)
                    .WhereIf(request.CourseId.HasValue,
                            l => l.Lesson.StudentGroup.CourseId == request.CourseId)
                    .WhereIf(request.StartDate.HasValue,
                            d => d.PublishingDate >= request.StartDate)
                    .WhereIf(request.FinishDate.HasValue,
                            d => d.PublishingDate <= request.FinishDate);
    }
}
