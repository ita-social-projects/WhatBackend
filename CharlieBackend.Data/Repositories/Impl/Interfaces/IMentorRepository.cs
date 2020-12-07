using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IMentorRepository : IRepository<Mentor>
    {
        public new Task<List<Mentor>> GetAllAsync();

        public Task<List<Mentor>> GetAllActiveAsync();

        void UpdateMentorCourses(IEnumerable<MentorOfCourse> currentItems,
                                 IEnumerable<MentorOfCourse> newItems);

        void UpdateMentorGroups(IEnumerable<MentorOfStudentGroup> currentItems,
                                IEnumerable<MentorOfStudentGroup> newItems);

        Task<List<Mentor>> GetAllActiveAsync();

        Task<Mentor> GetMentorByAccountIdAsync(long accountId);

        Task<Mentor> GetMentorByIdAsync(long mentorId);

        Task<List<Mentor>> GetMentorsByIdsAsync(IList<long> mentorIds);

    }
}
