using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using CharlieBackend.Data.Helpers;

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
                    .ToListAsync();
        }

        public async Task<List<Student>> GetAllActiveAsync()
        {
            return await _applicationContext.Students
                    .Include(student => student.Account).ThenInclude(x => x.Avatar)
                    .Where(student => student.Account.IsActive == true)
                    .ToListAsync();
        }

        public new async Task<Student> GetByIdAsync(long id)
        {
            return await _applicationContext.Students
                    .Include(student => student.Account)
                    .Include(student => student.StudentsOfStudentGroups)
                    .FirstOrDefaultAsync(student => student.Id == id);
        }

        public async Task<Student> GetStudentByAccountIdAsync(long accountId)
        {
            return await _applicationContext.Students
                    .Include(x => x.StudentsOfStudentGroups)
                    .FirstOrDefaultAsync(student => student.AccountId == accountId);
        }

        public async Task<List<Student>> GetStudentsByIdsAsync(IList<long> studentIds)
        {
            return await _applicationContext.Students
                    .Where(student => studentIds.Contains(student.Id))
                    .ToListAsync();
        }

        public async Task<Student> GetStudentByEmailAsync(string email)
        {
            return await _applicationContext.Students
                    .Include(student => student.Account)
                    .FirstOrDefaultAsync(student => 
                    student.Account.Email == email && student.Account.Role.HasFlag(UserRole.Student));
        }
    }
}
