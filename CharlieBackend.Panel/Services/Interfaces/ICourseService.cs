using CharlieBackend.Core.DTO.Course;
using CharlieBackend.Panel.Models.Course;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IList<CourseViewModel>> GetAllCoursesAsync();

        Task AddCourseAsync(CreateCourseDto courseDto);

        Task<CourseDto> DisableCourseAsync(long id);

        Task<CourseDto> EnableCourseAsync(long id);

        Task UpdateCourse(long id, UpdateCourseDto UpdateDto);
    }
}
