using CharlieBackend.AdminPanel.Models.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Services.Interfaces
{
    public interface ICourseService
    {
        public Task<IList<CourseViewModel>> GetAllCoursesAsync(string accessToken);
    }
}
