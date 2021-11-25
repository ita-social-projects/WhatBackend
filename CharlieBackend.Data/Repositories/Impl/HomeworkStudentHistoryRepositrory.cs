using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
