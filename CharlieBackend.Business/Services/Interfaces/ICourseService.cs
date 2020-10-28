using CharlieBackend.Core.DTO.Course;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface ICourseService
    {
        public Task<CreateCourseDto> CreateCourseAsync(CreateCourseDto courseModel);

        public Task<IList<CourseDto>> GetAllCoursesAsync();

        public Task<UpdateCourseDto> UpdateCourseAsync(long id, UpdateCourseDto courseModel);

        public Task<bool> IsCourseNameTakenAsync(string courseName);
    }
}
