using CharlieBackend.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface ICourseService
    {
        public Task<CourseModel> CreateCourseAsync(CourseModel courseModel);
        public Task<List<CourseModel>> GetAllCoursesAsync();
    }
}
