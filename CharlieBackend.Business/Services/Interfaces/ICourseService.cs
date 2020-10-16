using CharlieBackend.Core.Models.Course;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface ICourseService
    {
        public Task<CourseModel> CreateCourseAsync(CourseModel courseModel);

        public Task<List<CourseModel>> GetAllCoursesAsync();

        public Task<CourseModel> UpdateCourseAsync(CourseModel courseModel);

        public Task<bool> IsCourseNameTakenAsync(string courseName);
    }
}
