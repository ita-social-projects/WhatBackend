using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Data.Helpers;
using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Linq;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class MentorRepository : Repository<Mentor>, IMentorRepository
    {
        public MentorRepository(ApplicationContext applicationContext) 
                : base(applicationContext)
        {
        }

        public new Task<List<Mentor>> GetAllAsync()
        {
            return _applicationContext.Mentors
                .Include(mentor => mentor.Account)
                .Include(mentor => mentor.MentorsOfCourses)
                .ToListAsync();
        }

        public Task<List<Mentor>> GetMentorsByIdsAsync(IList<long> mentorIds)
        {
            return _applicationContext.Mentors
                    .Where(mentor => mentorIds.Contains(mentor.Id))
                    .ToListAsync();
        }

        public new Task<Mentor> GetByIdAsync(long id)
        {
            return _applicationContext.Mentors
                .Include(mentor => mentor.Account)
                .Include(mentor => mentor.MentorsOfCourses)
                .Include(mentor => mentor.MentorsOfStudentGroups)
                .FirstOrDefaultAsync(mentor => mentor.Id == id);
        }

        public void UpdateMentorCourses(IEnumerable<MentorOfCourse> currentItems,
                                        IEnumerable<MentorOfCourse> newItems)
        {
            _applicationContext.MentorsOfCourses
                    .TryUpdateManyToMany(currentItems, newItems);
        }

        public void UpdateMentorGroups(IEnumerable<MentorOfStudentGroup> currentItems,
                                       IEnumerable<MentorOfStudentGroup> newItems)
        {
            _applicationContext.MentorsOfStudentGroups.
                    TryUpdateManyToMany(currentItems, newItems);
        }

        public Task<Mentor> GetMentorByAccountIdAsync(long mentorId)
        {
            return _applicationContext.Mentors
                    .FirstOrDefaultAsync(mentor
                            => mentor.AccountId == mentorId);
        }
        public Task<Mentor> GetMentorByIdAsync(long mentorId)
        {
            return _applicationContext.Mentors
                    .FirstOrDefaultAsync(mentor
                            => mentor.Id == mentorId);
        }
    }
}
