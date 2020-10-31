using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Helpers;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class StudentGroupRepository : Repository<StudentGroup>, IStudentGroupRepository
    {
        public StudentGroupRepository(ApplicationContext applicationContext) 
            : base(applicationContext) 
        {
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

        public new Task<List<StudentGroup>> GetAllAsync()
        {
            return _applicationContext.StudentGroups
                    .Include(group => group.StudentsOfStudentGroups)
                    .Include(group => group.MentorsOfStudentGroups).ToListAsync();
        }

        public async Task<bool> IsGroupNameChangableAsync(string name)
        {
            var groups = await _applicationContext.StudentGroups
                    .Where(grName => grName.Name == name).ToListAsync();

            if (groups.Count > 1)
            {
                return false;
            }
           
            return true;
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

        public new Task<StudentGroup> GetByIdAsync(long id)
        {
            return _applicationContext.StudentGroups
                    .Include(group => group.StudentsOfStudentGroups)
                    .Include(group => group.MentorsOfStudentGroups)
                    .FirstOrDefaultAsync(group => group.Id == id);
        }
    }
}
