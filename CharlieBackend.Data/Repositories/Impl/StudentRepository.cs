using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(ApplicationContext applicationContext) : base(applicationContext) { }

        public new Task<List<Student>> GetAllAsync()
        {
            return _applicationContext.Students
                .Include(student => student.Account)
                //.Include(student => student.StudentsOfStudentGroups)
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
            return _applicationContext.Students.FirstOrDefaultAsync(student => student.AccountId == accountId);
        }

    }
}
