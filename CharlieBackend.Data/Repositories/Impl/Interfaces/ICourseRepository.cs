using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Mentor;
using CharlieBackend.Core.Models.ResultModel;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<bool> IsCourseNameTakenAsync(string courseName);

        Task<List<MentorCoursesDto>> GetMentorCourses(long id);

        Task<List<Course>> GetCoursesByIdsAsync(List<long> courseIds);
        
        Task<Result<Course>> DisableCourseByIdAsync(long id);

        Task<Result<Course>> EnableCourseByIdAsync(long id);

        Task<bool> DoesMentorHasAccessToCourse(long mentorId, long courseId);

        Task<bool> IsCourseHasGroupAsync(long id);

        Task<IList<Course>> GetCoursesAsync(bool? isActive);

        Task<bool> IsCourseActive(long id);
    }
}
