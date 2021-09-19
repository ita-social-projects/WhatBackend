using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IMentorOfCourseRepository : IRepository<MentorOfCourse>
    {
        Task<List<MentorOfCourse>> GetAllMentorCoursesAsync(long mentorId);

        Task<MentorOfCourse> GetMentorOfCourseIdAsync(MentorOfCourse mentorOfCourse);
        Task<IList<long?>> GetMentorCoursesById(long mentorId);
        Task<long> GetCoursesAsync(long courseId);
    }
}
