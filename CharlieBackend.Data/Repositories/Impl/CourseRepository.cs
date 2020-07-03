using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationContext applicationContext) : base(applicationContext) { }
        public Task<bool> IsCourseNameTakenAsync(string courseName)
        {
            return _applicationContext.Courses.AnyAsync(course => course.Name == courseName);
        }
    }
}
