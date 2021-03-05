using CharlieBackend.AdminPanel.Models.Course;
using CharlieBackend.Core.DTO.Course;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IList<CourseViewModel>> GetAllCoursesAsync();

        Task<bool> DisableCourseAsync(long id);

        Task UpdateCourse(long id, UpdateCourseDto UpdateDto);
    }
}
