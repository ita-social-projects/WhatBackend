using CharlieBackend.Core.DTO.Course;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface ICourseService
    {
        public Task<CourseDto> CreateCourseAsync(CreateCourseDto courseModel);

        public Task<IList<CourseDto>> GetAllCoursesAsync();

        public Task<CourseDto> UpdateCourseAsync(long id, UpdateCourseDto courseModel);

        public Task<bool> IsCourseNameTakenAsync(string courseName);

        public Task<bool> IsCourseEmptyAsync(long id);
        public Task<bool> DisableCourceAsync(long id);
    }

}
