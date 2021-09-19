using System;
using System.Collections.Generic;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Text;
using CharlieBackend.Core.Entities;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class HomeworkStudentHistoryRepositrory : Repository<HomeworkStudentHistory>, IHomeworkStudentHistoryRepository
    {
        public HomeworkStudentHistoryRepositrory(ApplicationContext context)
           : base(context)
        {
        }

        public async Task<IList<HomeworkStudentHistory>> GetHomeworkStudentHistoryByHomeworkStudentId(long homeworkStudentId)
        {
            return await _applicationContext.HomeworkStudentsHistory
                .Include(x => x.Mark)
                .Include(x => x.HomeworkStudent)
                .Include(x => x.AttachmentOfHomeworkStudentsHistory)
                .Where(x => x.HomeworkStudentId == homeworkStudentId)
                .OrderBy(x => x.PublishingDate)
                .ToListAsync();
        }
        public async Task<HomeworkStudentHistory> GetHomeworkStudentHistory(long groupId, long studentId, long homeworkStudentId)
        {
            return await _applicationContext.HomeworkStudentsHistory
                .Include(x => x.Mark)
                .Include(x => x.HomeworkStudent)
                .Include(x => x.AttachmentOfHomeworkStudentsHistory)
                .Where(x => x.HomeworkStudentId == homeworkStudentId)
                .Where(x => x.HomeworkStudent.StudentId == studentId && x.HomeworkStudent.Homework.Lesson.StudentGroupId == groupId)
                .OrderBy(x => x.PublishingDate)
                .LastOrDefaultAsync();
        }


    }
}
