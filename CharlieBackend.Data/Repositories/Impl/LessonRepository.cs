using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class LessonRepository : Repository<Lesson>, ILessonRepository
    {
        public LessonRepository(ApplicationContext applicationContext) : base(applicationContext) { }

        public new Task<List<Lesson>> GetAllAsync()
        {
            return _applicationContext.Lessons
                .Include(lesson => lesson.Visits)
                .Include(lesson => lesson.Theme)
                .ToListAsync();
        }
    }
}
