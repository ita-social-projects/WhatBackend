using CharlieBackend.Core.DTO.Course;
using CharlieBackend.Core.Models.ResultModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface ICourseService
    {
        Task<Result<CourseDto>> CreateCourseAsync(CreateCourseDto courseModel);

        Task<IList<CourseDto>> GetAllActiveCoursesAsync();

        Task<IList<CourseDto>> GetAllCoursesAsync();

        Task<Result<CourseDto>> UpdateCourseAsync(long id, UpdateCourseDto courseModel);

        Task<bool> IsCourseNameTakenAsync(string courseName);
        Task<Result<bool>> DisableCourceAsync(long id);
    }

}
