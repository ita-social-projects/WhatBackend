using CharlieBackend.AdminPanel.Models.Course;
using CharlieBackend.Core.DTO.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IList<CourseViewModel>> GetAllCoursesAsync();

        Task<CourseDto> DisableCourseAsync(long id);

        Task UpdateCourse(long id, UpdateCourseDto UpdateDto);
    }
}
