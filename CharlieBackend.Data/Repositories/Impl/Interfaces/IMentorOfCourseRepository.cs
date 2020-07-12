using CharlieBackend.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IMentorOfCourseRepository : IRepository<MentorOfCourse>
    {
        Task<List<MentorOfCourse>> GetAllMentorCoursesAsync(long mentorId);
        Task<MentorOfCourse> GetMentorOfCourseIdAsync(MentorOfCourse mentorOfCourse);
    }
}
