using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Data.Helpers;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core;
using Microsoft.EntityFrameworkCore;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using CharlieBackend.Core.DTO.Homework;

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

        public async Task<Homework> GetMentorHomeworkAsync(long mentorId, long homeworkId)
        {
            return await _applicationContext.Homeworks
                .Include(x => x.Lesson)
                .FirstOrDefaultAsync(x => (x.Id == homeworkId) && (x.Lesson.MentorId == mentorId));
        }

        public async Task<IList<Homework>> GetMentorFilteredHomwork(
                HomeworkFilterDto filter, long? mentorId)
        {
            return await _applicationContext.Mentors
                    .Include(m => m.Lesson).ThenInclude(l => l.StudentGroup)
                    .SelectMany(m => m.Lesson
                            .Where(l => l.MentorId == mentorId))
                    .WhereIf(filter.GroupId != default,
                            l => l.StudentGroupId == filter.GroupId)
                    .WhereIf((filter.CourseId != default)
                            && (filter.GroupId == default),
                            l => l.StudentGroup.CourseId == filter.CourseId)                
                    .Include(l => l.Homeworks)
                    .SelectMany(l => l.Homeworks
                            .Where(h => h.LessonId == l.Id))
                    .WhereIf(filter.StartDate != default,
                            h => h.DueDate >= filter.StartDate)
                    .WhereIf(filter.FinishDate != default,
                            h => h.DueDate <= filter.FinishDate)
                    .ToListAsync();  
        }
    }
}
