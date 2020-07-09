using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.Account;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(ApplicationContext applicationContext) : base(applicationContext) { }

        public Task<Account> GetAccountCredentials(AuthenticationModel authenticationModel)
        {
            return _applicationContext.Accounts.FirstOrDefaultAsync(account => account.Email == authenticationModel.Email &&
                                                                    account.Password == authenticationModel.Password);
        }

        public async Task<string> GetAccountSaltByEmail(string email)
        {
            var account = await _applicationContext.Accounts.FirstOrDefaultAsync(account => account.Email == email);
            if (account == null) return "";
            return account.Salt;
        }

        public async Task<string> GetAccountSaltById(long id)
        {
            var account = await _applicationContext.Accounts.FirstOrDefaultAsync(account => account.Id == id);
            if (account == null) return "";
            return account.Salt;
        }

        public Task<bool> IsEmailTakenAsync(string email)
        {
            return _applicationContext.Accounts.AnyAsync(account => account.Email == email);
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
            var count = await _applicationContext.Accounts.Where(account => account.Email == newEmail).CountAsync();
            if (count > 1) return false;
            return true;
        }

        public async Task<bool> IsAccountActiveAsync(string email)
        {
            var foundAccount = await _applicationContext.Accounts.FirstOrDefaultAsync(account => account.Email == email);
            return (bool)foundAccount.IsActive;
        }

        public async Task DisableAccountAsync(string email)
        {
            var foundAccount = await _applicationContext.Accounts.FirstOrDefaultAsync(account => account.Email == email);
            foundAccount.IsActive = false;
        }
    }
}
