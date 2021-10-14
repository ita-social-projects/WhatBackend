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

        public new async Task<List<Mentor>> GetAllAsync()
        {
            return await _applicationContext.Mentors
                .Include(mentor => mentor.Account).ThenInclude(x=>x.Avatar)
                .Include(mentor => mentor.MentorsOfCourses)
                .Where(mentor => mentor.Account.Role.HasFlag(UserRole.Mentor)
)
                .ToListAsync();
        }

        public async Task<List<Mentor>> GetAllActiveAsync()
        {
            return await _applicationContext.Mentors
                    .Include(mentor => mentor.Account).ThenInclude(x => x.Avatar)
                    .Where(mentor => mentor.Account.IsActive == true && mentor.Account.Role.HasFlag(UserRole.Mentor))
                    .ToListAsync();
        }

        public async Task<List<Mentor>> GetMentorsByIdsAsync(IList<long> mentorIds)
        {
            return await _applicationContext.Mentors
                    .Where(mentor => mentorIds.Contains(mentor.Id) && mentor.Account.Role.HasFlag(UserRole.Mentor))
                    .ToListAsync();
        }

        public new async Task<Mentor> GetByIdAsync(long id)
        {
            return await _applicationContext.Mentors
                .Include(mentor => mentor.Account)
                .Include(mentor => mentor.MentorsOfCourses)
                .Include(mentor => mentor.MentorsOfStudentGroups)
                .FirstOrDefaultAsync(mentor => mentor.Id == id && mentor.Account.Role.HasFlag(UserRole.Mentor));
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

        public async Task<Mentor> GetMentorByAccountIdAsync(long mentorId)
        {
            return await _applicationContext.Mentors
                    .FirstOrDefaultAsync(mentor
                            => mentor.AccountId == mentorId);
        }
        public async Task<Mentor> GetMentorByIdAsync(long mentorId)
        {
            return await _applicationContext.Mentors
                    .FirstOrDefaultAsync(mentor
                            => mentor.Id == mentorId && mentor.Account.Role.HasFlag(UserRole.Mentor));
        }
    }
}
