using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class MentorRepository : Repository<Mentor>, IMentorRepository
    {
        public MentorRepository(ApplicationContext applicationContext) : base(applicationContext) { }

        public new Task<List<Mentor>> GetAllAsync()
        {
            return _applicationContext.Mentors.Include(mentor => mentor.Account).ToListAsync();
        }
    }
}
