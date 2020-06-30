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
    }
}
