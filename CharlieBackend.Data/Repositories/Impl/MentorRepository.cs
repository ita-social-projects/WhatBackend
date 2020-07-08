using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Helpers;
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
            return _applicationContext.Mentors
                .Include(mentor => mentor.Account)
                .Include(mentor => mentor.MentorsOfCourses)
                .ToListAsync();
        }

        public new Task<Mentor> GetByIdAsync(long id)
        {
            return _applicationContext.Mentors
                .Include(mentor => mentor.Account)
                .Include(mentor => mentor.MentorsOfCourses)
                .FirstOrDefaultAsync(mentor => mentor.Id == id);
        }

        public void UpdateManyToMany(IEnumerable<MentorOfCourse> currentItems, IEnumerable<MentorOfCourse> newItems)
        {
            _applicationContext.MentorsOfCourses.TryUpdateManyToMany(currentItems, newItems);
        }

        //public Task<Mentor> GetMentorByAccountIdAsync(long mentorId)
        //{
        //    return _applicationContext.Mentors.FirstOrDefaultAsync(mentor => mentor.id == mentorId);
        //}
    }
}
