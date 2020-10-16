using CharlieBackend.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IStudentRepository : IRepository<Student>
    {
        public new Task<List<Student>> GetAllAsync();

        Task<Student> GetStudentByAccountIdAsync(long accountId);

        Task<List<Student>> GetStudentsByIdsAsync(List<long> studentIds);

        Task<Student> GetStudentByEmailAsync(string email);
    }
}
