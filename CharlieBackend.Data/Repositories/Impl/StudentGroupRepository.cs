using CharlieBackend.Core;
using CharlieBackend.Core.DTO.Mentor;
using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Helpers;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Z.EntityFramework.Plus;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class StudentGroupRepository : Repository<StudentGroup>, IStudentGroupRepository
    {
        public StudentGroupRepository(ApplicationContext applicationContext)
            : base(applicationContext)
        {
        }

        public async void AddStudentOfStudentGroups(IEnumerable<StudentOfStudentGroup> items)
        {
            await _applicationContext.StudentsOfStudentGroups
                   .AddRangeAsync(items);
        }

        public async Task<bool> DeleteStudentGroupAsync(long StudentGroupModelId)
        {
            var x = await GetByIdAsync(StudentGroupModelId);

            if (x == null)
            {
                return false;
            }
            else
            {
                _applicationContext.StudentGroups.Remove(x);

                return true;
            }
        }

        public async Task<bool> DeactivateStudentGroupAsync(long StudentGroupModelId)
        {
            var x = await GetByIdAsync(StudentGroupModelId);

            if (x == null)
            {
                return false;
            }
            else
            {
                x.IsActive = false;
                _applicationContext.StudentGroups.Update(x);
                
                return true;
            }
        }

        public new async Task<List<StudentGroup>> GetAllAsync()
        {
            return await _applicationContext.StudentGroups
                    .Include(group => group.StudentsOfStudentGroups)
                    .Include(group => group.MentorsOfStudentGroups).ToListAsync();
        }

        public async Task<IList<StudentGroup>> GetAllActiveAsync(DateTime? startDate, DateTime? finishDate)
        {
            return await _applicationContext.StudentGroups
            .IncludeFilter(group => group.StudentsOfStudentGroups.Where(s => s.Student.Account.Role.HasFlag(UserRole.Student) && s.Student.Account.IsActive.Value))
            .IncludeFilter(group => group.MentorsOfStudentGroups.Where(m => m.Mentor.Account.Role.HasFlag(UserRole.Mentor) && m.Mentor.Account.IsActive.Value))
            .WhereIf(!(startDate is null), group => group.StartDate >= startDate ||
                                                    group.FinishDate > startDate)
            .WhereIf(!(finishDate is null), group => group.FinishDate < finishDate ||
                                                     group.StartDate < finishDate)
            .Where(group => group.Course.IsActive)
            .Where(group => group.IsActive)
            .OrderBy(group => group.StartDate)
            .ToListAsync();
        }

        public async Task<StudentGroup> GetActiveStudentGroupByIdAsync(long id)
        {
            return await _applicationContext.StudentGroups
                    .IncludeFilter(group => group.StudentsOfStudentGroups.Where(s => s.Student.Account.Role.HasFlag(UserRole.Student) && s.Student.Account.IsActive.Value))
                    .IncludeFilter(group => group.MentorsOfStudentGroups.Where(m => m.Mentor.Account.Role.HasFlag(UserRole.Mentor) && m.Mentor.Account.IsActive.Value))
                    .FirstOrDefaultAsync(group => group.Id == id && group.IsActive);
        }

        public async Task<List<StudentStudyGroupsDto>> GetStudentStudyGroups(long id)
        {
            return await _applicationContext.StudentGroups
                    .Where(group => group.IsActive)
                    .Include(group => group.StudentsOfStudentGroups)
                    .Where(x => x.StudentsOfStudentGroups.Any(x => x.StudentId == id))
                    .Select(x => new StudentStudyGroupsDto
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToListAsync();
        }

        public async Task<bool> IsGroupNameExistAsync(string name)
        {
            return await _applicationContext.StudentGroups.AnyAsync(x => x.Name == name);
        }

        public async Task<bool> IsGroupOnCourseAsync(long id)
        {
            return await _applicationContext.StudentGroups.AnyAsync(group => (group.CourseId == id) && (group.FinishDate >= DateTime.Now));
        }

        public void UpdateManyToMany(IEnumerable<StudentOfStudentGroup> currentStudentsOfStudentGroup,
                                     IEnumerable<StudentOfStudentGroup> newStudentsOfStudentGroup)
        {
            _applicationContext.StudentsOfStudentGroups.
                    TryUpdateManyToMany(currentStudentsOfStudentGroup, newStudentsOfStudentGroup);
        }

        public new async Task<StudentGroup> GetByIdAsync(long id)
        {
            return await _applicationContext.StudentGroups
                    .Include(group => group.StudentsOfStudentGroups)
                    .Include(group => group.MentorsOfStudentGroups)
                    .FirstOrDefaultAsync(group => group.Id == id);
        }

        public async Task<List<MentorStudyGroupsDto>> GetMentorStudyGroups(long id)
        {
            return await _applicationContext.StudentGroups
                    .Where(group => group.IsActive)
                    .Include(group => group.MentorsOfStudentGroups)
                    .Where(x => x.MentorsOfStudentGroups.Any(x => x.MentorId == id))
                    .Select(x => new MentorStudyGroupsDto
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToListAsync();
        }

        public async Task<bool> DoesMentorHaveAccessToGroup(long mentorId, long groupId)
        {
            return await _applicationContext.StudentGroups
                    .Include(group => group.MentorsOfStudentGroups)
                    .AnyAsync(x => x.MentorsOfStudentGroups.Any(x => x.MentorId == mentorId) && x.Id == groupId);
        }

        public async Task<IList<long?>> GetGroupStudentsIds(long id)
        {
            return await _applicationContext.StudentsOfStudentGroups
                    .Where(s => s.StudentGroupId == id)
                    .Select(s => s.StudentId).ToListAsync();
        }

        public async Task<IList<Student>> GetGroupStudentsByGroupId(long id)
        {
            return await _applicationContext.StudentsOfStudentGroups.Where(s => s.StudentGroupId == id)
                .Include(x => x.Student)
                .ThenInclude(x => x.Account)
                .Select(s => s.Student)
                .ToListAsync();
        }

        public async Task<IList<StudentGroup>> GetStudentGroupsByDateAsync(DateTime? startDate, DateTime? finishDate)
        {
            return await _applicationContext.StudentGroups
                .AsNoTracking()
                .Include(group => group.StudentsOfStudentGroups)
                .Include(group => group.MentorsOfStudentGroups)
                .WhereIf(!(startDate is null), group => group.StartDate >= startDate ||
                                                        group.FinishDate > startDate)
                .WhereIf(!(finishDate is null), group => group.FinishDate < finishDate || 
                                                         group.StartDate < finishDate)
                .Where(group => group.Course.IsActive)
                .OrderBy(group => group.StartDate)
                .ToListAsync();
        }

        public async Task<IList<long?>> GetStudentGroupsIdsByStudentId(long id)
        {
            return await _applicationContext.StudentsOfStudentGroups
                .Where(s => s.StudentId == id)
                .Select(s => s.StudentGroupId)
                .ToListAsync();
        }

        public async Task<bool> DoesStudentBelongsGroup(long studentId, long studentGroupId)
        {
            return await _applicationContext.StudentsOfStudentGroups.AnyAsync(s => s.StudentGroupId == studentGroupId && s.StudentId == studentId);
        }
    }
}
