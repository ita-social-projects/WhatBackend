using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using CharlieBackend.Core.Models.ResultModel;


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

        public async Task<bool> IsCourseHasGroupAsync(long id)
        {
            return await _applicationContext.StudentGroups.AnyAsync(s => s.CourseId == id);
        }

        public async Task<Result<bool>> DisableCourseByIdAsync(long id)
        {
            var course = await _applicationContext.Courses.FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
           
            if (course == null)
            {
                return Result<bool>.GetError(ErrorCode.NotFound,"Course is not found");
            }
            course.IsActive = false;

            return Result<bool>.GetSuccess(true);
        }
    }
}
