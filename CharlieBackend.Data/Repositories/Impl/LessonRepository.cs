using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class LessonRepository : Repository<Lesson>, ILessonRepository
    {
        public LessonRepository(ApplicationContext applicationContext) : base(applicationContext) { }
    }
}
