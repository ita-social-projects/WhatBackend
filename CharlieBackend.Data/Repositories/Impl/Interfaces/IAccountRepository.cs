using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        public Task<Account> GetAccountCredentials(AuthenticationModel authenticationModel);
        public Task<string> GetAccountSalt(string email);
        public Task<bool> IsEmailTakenAsync(string email);
    }
}
