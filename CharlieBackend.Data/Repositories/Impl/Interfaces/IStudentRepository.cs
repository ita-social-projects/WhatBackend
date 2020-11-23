using CharlieBackend.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IStudentRepository : IRepository<Student>
    {
        new Task<List<Student>> GetAllAsync();

        Task<List<Student>> GetAllActiveAsync();

        Task<Student> GetStudentByAccountIdAsync(long accountId);

        Task<List<Student>> GetStudentsByIdsAsync(IList<long> studentIds);

        Task<Student> GetStudentByEmailAsync(string email);

        bool IsStudentExist(long studentId);
    }
}
