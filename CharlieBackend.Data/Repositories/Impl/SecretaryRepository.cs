using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class SecretaryRepository : Repository<Secretary>, ISecretaryRepository
    {
        public SecretaryRepository(ApplicationContext applicationContext)
                : base(applicationContext)
        {
        }

        public new async Task<List<Secretary>> GetAllAsync()
        {
            return await _applicationContext.Secretaries
                .Include(secretary => secretary.Account).ThenInclude(x => x.Avatar)
                .Where(secretary => secretary.Account.Role.HasFlag(UserRole.Secretary))
                .ToListAsync();
        }
        public async Task<Secretary> GetSecretaryByAccountIdAsync(long accountId)
        {
            return await _applicationContext.Secretaries
                    .FirstOrDefaultAsync(secretary
                            => secretary.AccountId == accountId);
        }

        public new async Task<Secretary> GetByIdAsync(long id)
        {
            return await _applicationContext.Secretaries
                .Include(secretary => secretary.Account)
                .Where(secretary => secretary.Account.Role.HasFlag(UserRole.Secretary))
                .FirstOrDefaultAsync(secretary => secretary.Id == id);
        }

        public async Task<List<Secretary>> GetActiveAsync()
        {
            return await _applicationContext.Secretaries
                .Include(secretary => secretary.Account).ThenInclude(x => x.Avatar)
                .Where(sec => sec.Account.IsActive == true && sec.Account.Role.HasFlag(UserRole.Secretary))
                .Select(sec => sec)
                .ToListAsync();
        }
    }
}
