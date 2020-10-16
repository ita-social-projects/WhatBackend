using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IMentorRepository : IRepository<Mentor>
    {
        public new Task<List<Mentor>> GetAllAsync();

        void UpdateMentorCourses(IEnumerable<MentorOfCourse> currentItems,
                                 IEnumerable<MentorOfCourse> newItems);

        void UpdateMentorGroups(IEnumerable<MentorOfStudentGroup> currentItems,
                                IEnumerable<MentorOfStudentGroup> newItems);

        //public Task<Mentor> GetAccountByMentorIdAsync(long mentorId);

        Task<Mentor> GetMentorByAccountIdAsync(long accountId);
    }
}
