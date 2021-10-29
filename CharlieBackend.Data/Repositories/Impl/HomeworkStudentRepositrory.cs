using CharlieBackend.Core;
using CharlieBackend.Core.DTO.HomeworkStudent;
using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Helpers;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class HomeworkStudentRepositrory : Repository<HomeworkStudent>, IHomeworkStudentRepository
    {
        public HomeworkStudentRepositrory(ApplicationContext context)
            :base (context) 
        {

        }

        public async override Task<HomeworkStudent> GetByIdAsync(long id)
        {
            return await _applicationContext.HomeworkStudents
                .Include(x => x.AttachmentOfHomeworkStudents)
                .Include(x => x.Homework)
                .Include(x => x.Student)
                .ThenInclude(x => x.Account)
                .Include(x => x.Mark)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsStudentHasHomeworkAsync(long studentId, long homeworkId)
        {
            return await _applicationContext.HomeworkStudents.AnyAsync(x => (x.StudentId == studentId) && (x.HomeworkId == homeworkId));
        }

        public async Task<IList<HomeworkStudent>> GetHomeworkForStudent(long studentId, DateTime? startDate, DateTime? finishDate, long groupId)
        {
            return await _applicationContext.HomeworkStudents
                .Include(x => x.AttachmentOfHomeworkStudents)
                .Include(x => x.Homework)
                .ThenInclude(x => x.Lesson)
                .Include(x => x.Student)
                .Include(x => x.Student.Account)
                .Include(x => x.Mark)
                .WhereIf(startDate != null && startDate != default(DateTime),
                 x => x.PublishingDate >= startDate)
                .WhereIf(finishDate != null && finishDate != default(DateTime),
                x => x.PublishingDate <= finishDate)
                .Where(x => x.StudentId == studentId && x.Homework.Lesson.StudentGroupId == groupId && x.IsSent == true)
                .ToListAsync();
        }

        public async Task<IList<HomeworkStudent>> GetHomeworkStudentForStudent(long studentId)
        {
            return await _applicationContext.HomeworkStudents
                .Include(x => x.AttachmentOfHomeworkStudents)
                .Include(x => x.Homework)
                .Include(x => x.Student)
                .Include(x => x.Student.Account)
                .Include(x => x.Mark)
                .Where(x => x.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<IList<HomeworkStudent>> GetHomeworkStudentForMentor(long homeworkId)
        {
            return await _applicationContext.HomeworkStudents
                .Include(x => x.AttachmentOfHomeworkStudents)
                .Include(x => x.Homework)
                .Include(x => x.Student)
                .ThenInclude(x => x.Account)
                .Include(x => x.Mark)
                .Where(x => x.HomeworkId == homeworkId)
                .ToListAsync();

        }

        public void UpdateManyToMany(IEnumerable<AttachmentOfHomeworkStudent> currentHomeworkAttachments,
                            IEnumerable<AttachmentOfHomeworkStudent> newHomeworkAttachments)
        {
            _applicationContext.AttachmentOfHomeworkStudents.TryUpdateManyToMany(currentHomeworkAttachments, newHomeworkAttachments);
        }
    }
}
