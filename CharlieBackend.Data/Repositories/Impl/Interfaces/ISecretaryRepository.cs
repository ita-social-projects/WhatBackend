using CharlieBackend.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface ISecretaryRepository : IRepository<Secretary>
    {
        public new Task<List<Secretary>> GetAllAsync();
     
        Task<Secretary> GetSecretaryByAccountIdAsync(long accountId);
      
        public new Task<Secretary> GetByIdAsync(long id);
       
        Task<List<Secretary>> GetActiveAsync();

    }
}
