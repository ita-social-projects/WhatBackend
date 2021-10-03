using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(ApplicationContext applicationContext)
            : base(applicationContext)
        {
        }
        public new async Task<List<Student>> GetAllAsync()
        {
            return await _applicationContext.Students
                    .Include(student => student.Account).ThenInclude(x => x.Avatar)
                    .Where(student => student.Account.Role.HasFlag(UserRole.Student))
                    .ToListAsync();
        }

        public async Task<List<Student>> GetAllActiveAsync()
        {
            return await _applicationContext.Students
                    .Include(student => student.Account).ThenInclude(x => x.Avatar)
                    .Where(student => student.Account.IsActive == true && student.Account.Role.HasFlag(UserRole.Student))
                    .ToListAsync();
        }

        public new async Task<Student> GetByIdAsync(long id)
        {
            return await _applicationContext.Students
                    .Include(student => student.Account)
                    .Include(student => student.StudentsOfStudentGroups)
                    .Where(student => student.Account.Role.HasFlag(UserRole.Student))
                    .FirstOrDefaultAsync(student => student.Id == id);
        }

        public async Task<Student> GetStudentByAccountIdAsync(long accountId)
        {
            return await _applicationContext.Students
                    .Where(student => student.Account.Role.HasFlag(UserRole.Student))
                    .FirstOrDefaultAsync(student => student.AccountId == accountId);
        }

        public async Task<List<Student>> GetStudentsByIdsAsync(IList<long> studentIds)
        {
            return await _applicationContext.Students
                    .Where(student => studentIds.Contains(student.Id) && student.Account.Role.HasFlag(UserRole.Student))
                    .ToListAsync();
        }

        public async Task<Student> GetStudentByEmailAsync(string email)
        {
            return await _applicationContext.Students
                    .Include(student => student.Account)
                    .Where(student => student.Account.Role.HasFlag(UserRole.Student))
                    .FirstOrDefaultAsync(student =>
                    student.Account.Email == email);
        }

        public async Task<IList<Student>> GetStudentsByIdGroups(long groupId)
        {
            return await _applicationContext.Students
                  .Join(_applicationContext.StudentsOfStudentGroups,
                        s => s.Id,
                        sOfS => sOfS.StudentId,
                        (s, sOfG) => new { Student = s, StudentOfGroup = sOfG })
                  .Where(related => related.StudentOfGroup.StudentGroupId == groupId)
                  .Select(related => related.Student)
                  .Include(s => s.Account)
                  .Where(student => student.Account.Role.HasFlag(UserRole.Student))
                  .ToListAsync();
        }
    }
}
