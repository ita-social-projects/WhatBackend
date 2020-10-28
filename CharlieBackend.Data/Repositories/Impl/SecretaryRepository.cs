using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Data.Helpers;
using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Linq;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class SecretaryRepository : Repository<Secretary>, ISecretaryRepository
    {
        public SecretaryRepository(ApplicationContext applicationContext)
                : base(applicationContext)
        {
        }

        public new Task<List<Secretary>> GetAllAsync()
        {
            return _applicationContext.Secretaries
                .Include(secretary => secretary.Account)
                .ToListAsync();
        }
        public Task<Secretary> GetSecretaryByAccountIdAsync(long accountId)
        {
            return _applicationContext.Secretaries
                    .FirstOrDefaultAsync(secretary
                            => secretary.AccountId == accountId);
        }

    }
}



