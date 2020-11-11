using System.Linq;
using System.Threading.Tasks;
using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Data.Repositories.Impl.Interfaces;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(ApplicationContext applicationContext) 
                : base(applicationContext) 
        { 
        }

        public Task<Account> GetAccountCredentials(AuthenticationDto authenticationModel)
        {
            return _applicationContext.Accounts
                .FirstOrDefaultAsync(account 
                         => account.Email == authenticationModel.Email 
                                && account.Password == authenticationModel.Password);
        }

		public Task<Account> GetAccountCredentialsById(long id)
        {
            return _applicationContext.Accounts
                .FirstOrDefaultAsync(account => account.Id == id);
        }

        public async Task<string> GetAccountSaltByEmail(string email)
        {
            var account = await _applicationContext.Accounts
                    .FirstOrDefaultAsync(account => account.Email == email);
            if (account == null)
            {
                return "";
            }
            return account.Salt;
        }

        public async Task<string> GetAccountSaltById(long id)
        {
            var account = await _applicationContext.Accounts
                    .FirstOrDefaultAsync(account => account.Id == id);
            if (account == null)
            {
                return "";
            }
            return account.Salt;
        }

        public Task<bool> IsEmailTakenAsync(string email)
        {
            return _applicationContext.Accounts
                    .AnyAsync(account => account.Email == email);
        }

        public void UpdateAccountCredentials(Account account)
        {
            _applicationContext.Accounts.Attach(account);
            _applicationContext.Entry(account).Property(a => a.Email).IsModified = true;
            _applicationContext.Entry(account).Property(a => a.FirstName).IsModified = true;
            _applicationContext.Entry(account).Property(a => a.LastName).IsModified = true;
            _applicationContext.Entry(account).Property(a => a.Password).IsModified = true;
            _applicationContext.Entry(account).Property(a => a.Salt).IsModified = true;
        }

        public async Task<bool> IsEmailChangableToAsync(string newEmail)
        {
            var count = await _applicationContext.Accounts
                    .Where(account => account.Email == newEmail)
                    .CountAsync();
            if (count > 1) return false;
            return true;
        }

        public async Task<bool?> IsAccountActiveAsync(string email)
        {
            var foundAccount = await _applicationContext.Accounts
                    .FirstOrDefaultAsync(account => account.Email == email);
            return foundAccount?.IsActive;
        }

        public async Task<bool> DisableAccountAsync(long id)
        {
            var foundAccount = await _applicationContext.Accounts
                    .FirstOrDefaultAsync(account => account.Id == id);
            if (foundAccount == null)
            {
                return false;
            }
            foundAccount.IsActive = false;
            return false;
        }
    }
}
