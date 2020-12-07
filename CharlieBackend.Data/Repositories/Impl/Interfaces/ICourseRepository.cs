using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Mentor;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<bool> IsCourseNameTakenAsync(string courseName);

        Task<List<MentorCoursesDto>> GetMentorCourses(long id);

        Task<List<Course>> GetCoursesByIdsAsync(List<long> courseIds);
    }
}
