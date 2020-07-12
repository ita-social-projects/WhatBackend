using CharlieBackend.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IMentorRepository : IRepository<Mentor>
    {
        public new Task<List<Mentor>> GetAllAsync();
        void UpdateManyToMany(IEnumerable<MentorOfCourse> currentItems, IEnumerable<MentorOfCourse> newItems);
        //public Task<Mentor> GetAccountByMentorIdAsync(long mentorId);
        Task<Mentor> GetMentorByAccountIdAsync(long accountId);
    }
}
