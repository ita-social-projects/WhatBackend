using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using CharlieBackend.Data.Repositories.Impl.Interfaces;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(ApplicationContext applicationContext) 
            : base(applicationContext) 
        {
        }

        public new Task<List<Student>> GetAllAsync()
        {
            return _applicationContext.Students
                    .Include(student => student.Account)
                    .ToListAsync();
        }

        public Task<List<Student>> GetAllActiveAsync()
        {
            return _applicationContext.Students
                    .Include(student => student.Account)
                    .Where(student => student.Account.IsActive == true)
                    .ToListAsync();
        }

        public new Task<Student> GetByIdAsync(long id)
        {
            return _applicationContext.Students
                    .Include(student => student.Account)
                    .FirstOrDefaultAsync(student => student.Id == id);
        }

        public Task<Student> GetStudentByAccountIdAsync(long accountId)
        {
            return _applicationContext.Students
                    .FirstOrDefaultAsync(student => student.AccountId == accountId);
        }

        public Task<List<Student>> GetStudentsByIdsAsync(IList<long> studentIds)
        {
            return _applicationContext.Students
                    .Where(student => studentIds.Contains(student.Id))
                    .ToListAsync();
        }

        public async Task<Student> GetStudentByEmailAsync(string email)
        {
            return await _applicationContext.Students
                    .Include(student => student.Account)
                    .FirstOrDefaultAsync(student => 
                    student.Account.Email == email && student.Account.Role == UserRole.Student);
        }
    }
}
