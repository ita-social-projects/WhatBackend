using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using CharlieBackend.Data.Repositories.Impl.Interfaces;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class HometaskRepository : Repository<Hometask>, IHometaskRepository
    {
        public HometaskRepository(ApplicationContext applicationContext)
        : base(applicationContext)
        {
        }

        public async Task<IList<Hometask>> GetHometasksByCourseId(long courseId)
        {
            return await (from task in _applicationContext.Hometasks
                          .Include(x => x.AttachmentOfHometasks)
                          from lessons in task.Theme.Lessons
                          where lessons.StudentGroup.CourseId == courseId
                          select task).ToListAsync();
        }

        public async override Task<Hometask> GetByIdAsync(long hometaskId)
        {
            return await _applicationContext.Hometasks
                        .Include(x => x.AttachmentOfHometasks)
                        .FirstOrDefaultAsync(x => x.Id == hometaskId);
        }
    }
}
