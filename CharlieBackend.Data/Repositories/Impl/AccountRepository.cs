using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(ApplicationContext applicationContext) : base(applicationContext) { }

        public Task<Account> GetAccountCredentials(AuthenticationModel authenticationModel)
        {
            return _applicationContext.Accounts.FirstOrDefaultAsync(account => account.Email == authenticationModel.email &&
                                                                    account.Password == authenticationModel.password);
        }

        public async Task<string> GetAccountSalt(string email)
        {         
            var account = await _applicationContext.Accounts.FirstOrDefaultAsync(account => account.Email == email);
            if (account == null) return "";
            return account.Salt;
        } 

        public Task<bool> IsEmailTakenAsync(string email)
        {
           return _applicationContext.Accounts.AnyAsync(account => account.Email == email);
        }
    }
}
