﻿using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
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

        public Task<List<Account>> GetAllNotAssignedAsync()
        {
            return _applicationContext.Accounts
                .Where(account => account.Role == UserRole.NotAssigned)
                .ToListAsync();
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

        public async Task<bool> IsEmailChangableToAsync(long id, string newEmail)
        {
            var foundAccountOfEmail = await _applicationContext.Accounts
                    .FirstOrDefaultAsync(account => account.Email == newEmail);

            if (foundAccountOfEmail != null)
            {
                return foundAccountOfEmail.Id == id;
            }
            else
            {
                return true;
            }
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
            if (foundAccount == null || (bool)!foundAccount.IsActive)
            {
                return false;   
            }
            foundAccount.IsActive = false;
            return true;
        }

        public async Task<Account> GetAccountCredentialsByEmailAsync(string email)
        {
            return await _applicationContext.Accounts
                        .FirstOrDefaultAsync(account => account.Email == email);
        }
    }
}
