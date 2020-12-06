using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Mentor;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        public Task<bool> IsCourseNameTakenAsync(string courseName);

        public Task<List<MentorCoursesDto>> GetMentorCourses(long id);

        public Task<List<Course>> GetCoursesByIdsAsync(List<long> courseIds);
    }
}
