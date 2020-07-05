using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class MentorOfCourseRepository : Repository<MentorOfCourse>, IMentorOfCourseRepository
    {
        public MentorOfCourseRepository(ApplicationContext applicationContext) : base(applicationContext) { }
    }
}
