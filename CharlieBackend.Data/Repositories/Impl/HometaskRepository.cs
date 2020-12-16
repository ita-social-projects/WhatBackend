using CharlieBackend.Core.DTO.Homework;
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
    public class HometaskRepository : Repository<Hometask>, IHometaskRepository
    {
        public HometaskRepository(ApplicationContext applicationContext)
        : base(applicationContext)
        {
        }

        public async Task<IList<Hometask>> GetHometasksByCourseId(long courseId)
        {
            return await (from task in _applicationContext.Hometasks
                          from lessons in task.Theme.Lessons
                          where lessons.StudentGroup.CourseId == courseId
                          select task).ToListAsync();
        }
    }
}
