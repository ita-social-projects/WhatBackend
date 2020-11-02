using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface ISecretaryRepository : IRepository<Secretary>
    {
        public new Task<List<Secretary>> GetAllAsync();
        Task<Secretary> GetSecretaryByAccountIdAsync(long accountId);
        public new Task<Secretary> GetByIdAsync(long id);
    }
}
