using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Data.Helpers;
using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using CharlieBackend.Data.Repositories.Impl.Interfaces;

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

        public async Task<IList<Homework>> GetNotDoneHomeworksByStudentGroup(long studentGroup, List<long> studentHomeworkIds)
        {
            return await _applicationContext.Homeworks
                .Include(x => x.HomeworkStudents)
                .Include(x => x.Lesson)
                .ThenInclude(x => x.StudentGroup)
                .Where(x => x.Lesson.StudentGroup.Id == studentGroup && !studentHomeworkIds.Contains(x.Id))
                .ToListAsync();
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

    }
}
