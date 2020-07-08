using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public Task<List<Course>> GetCoursesByIdsAsync(List<long> courseIds)
        {
            return _applicationContext.Courses.Where(course => courseIds.Contains(course.Id)).ToListAsync();
        }
    }
}
