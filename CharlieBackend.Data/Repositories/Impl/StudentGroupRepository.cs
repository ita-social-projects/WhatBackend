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

        public bool DeleteStudentGroup(long StudentGroupModelId)
        {
            var x = SearchStudentGroup(StudentGroupModelId);

            if (x == null)
            {
                return false;
            }

            else
            {
                _applicationContext.StudentGroups.Remove(x);
                _applicationContext.SaveChanges();

                return true;
            }
        }

        public bool DeactivateStudentGroup(long StudentGroupModelId)
        {
            var x = SearchStudentGroup(StudentGroupModelId);

            if (x == null)
            {
                return false;
            }
            else
            {
                x.IsActive = false;
                _applicationContext.StudentGroups.Update(x);
                _applicationContext.SaveChanges();
                return true;
            }
        }

        public new async Task<List<StudentGroup>> GetAllAsync()
        {
            return await _applicationContext.StudentGroups
                    .Include(group => group.StudentsOfStudentGroups)
                    .Include(group => group.MentorsOfStudentGroups).ToListAsync();
        }

        public async Task<List<StudentStudyGroupsDto>> GetStudentStudyGroups(long id)
        {
            return await _applicationContext.StudentGroups
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

        public StudentGroup SearchStudentGroup(long studentGroupId)
        {
            foreach (var x in _applicationContext.StudentGroups)
            {
                if (x.Id == studentGroupId)
                {
                    return x;
                }
            }

            return null;
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
                    .Include(group => group.MentorsOfStudentGroups)
                    .Where(x => x.MentorsOfStudentGroups.Any(x => x.MentorId == id))
                    .Select(x => new MentorStudyGroupsDto
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToListAsync();
        }

        public async Task<IList<long?>> GetGroupStudentsIds(long id)
        {
            return await _applicationContext.StudentsOfStudentGroups.Where(s => s.StudentGroupId == id)
                .Select(s => s.StudentId).ToListAsync();
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

        public async Task<IList<long?>> GetStudentGroupsByStudentId(long id)
        {
            return await _applicationContext.StudentsOfStudentGroups
                .Where(s => s.StudentId == id)
                .Select(s => s.StudentGroupId)
                .ToListAsync();
        }
    }
}
