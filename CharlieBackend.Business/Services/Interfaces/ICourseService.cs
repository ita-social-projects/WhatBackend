using CharlieBackend.Core.DTO.Course;
using CharlieBackend.Core.Models.ResultModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface ICourseService
    {
        public Task<Result<CourseDto>> CreateCourseAsync(CreateCourseDto courseModel);

        public Task<Result<IList<CourseDto>>> GetAllCoursesAsync();

        public Task<Result<CourseDto>> UpdateCourseAsync(long id, UpdateCourseDto courseModel);

        public Task<bool> IsCourseNameTakenAsync(string courseName);

        public Task<bool> IsCourseEmptyAsync(long id);
        public Task<bool> DisableCourceAsync(long id);
    }

}
