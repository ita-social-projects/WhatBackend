﻿using System.Threading.Tasks;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.Account;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        public Task<Account> GetAccountCredentials(AuthenticationModel authenticationModel);

        public Task<string> GetAccountSaltByEmail(string email);

        public Task<string> GetAccountSaltById(long id);

        public Task<bool> IsEmailTakenAsync(string email);

        public Task<bool> IsEmailChangableToAsync(long? id, string newEmail);

        public Task<bool?> IsAccountActiveAsync(string email);

        Task<bool> DisableAccountAsync(long id);

        public void UpdateAccountCredentials(Account account);
    }
}
