using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using CharlieBackend.Core.DTO.Mentor;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationContext applicationContext) 
                : base(applicationContext) 
        {
        }

        public Task<bool> IsCourseNameTakenAsync(string courseName)
        {
            return _applicationContext.Courses
                    .AnyAsync(course => course.Name == courseName);
        }

        public Task<List<Course>> GetCoursesByIdsAsync(List<long> courseIds)
        {
            return _applicationContext.Courses
                    .Where(course => courseIds
                    .Contains(course.Id))
                    .ToListAsync();
        }

        public async Task<List<MentorCoursesDto>> GetMentorCourses(long id)
        {
            return await _applicationContext.Courses
                    .Where(x => x.MentorsOfCourses.Any(x => x.Id == id))
                    .Select(x => new MentorCoursesDto
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToListAsync();
        }
    }
}
