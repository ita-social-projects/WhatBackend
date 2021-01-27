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
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IList<HomeworkStudent>> GetHomeworkStudentForStudentByStudentId(long id)
        {
            return await _applicationContext.HomeworkStudents
                .Include(x => x.AttachmentOfHomeworkStudents)
                .Include(x => x.Homework)
                .Where(x => x.StudentId == id)
                .ToListAsync();
        }

        public async Task<IList<HomeworkStudent>> GetHomeworkStudentForMentorByHomeworkId(long homeworkId)
        {
            return await _applicationContext.HomeworkStudents
                .Include(x => x.AttachmentOfHomeworkStudents)
                .Include(x => x.Homework)
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
