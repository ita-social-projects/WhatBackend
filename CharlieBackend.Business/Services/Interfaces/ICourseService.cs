using CharlieBackend.Core.Models.Course;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface ICourseService
    {
        public Task<CreateCourseModel> CreateCourseAsync(CreateCourseModel courseModel);

        public Task<IList<CourseModel>> GetAllCoursesAsync();

        public Task<UpdateCourseModel> UpdateCourseAsync(long id, UpdateCourseModel courseModel);

        public Task<bool> IsCourseNameTakenAsync(string courseName);
    }
}
