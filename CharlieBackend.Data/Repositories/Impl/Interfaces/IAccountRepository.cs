using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        public Task<Account> GetAccountCredentials(AuthenticationDto authenticationModel);

        public Task<Account> GetAccountCredentialsById(long id);

        public Task<List<Account>> GetAllNotAssignedAsync();

        public Task<string> GetAccountSaltByEmail(string email);

        public Task<string> GetAccountSaltById(long id);

        public Task<bool> IsEmailTakenAsync(string email);

        public Task<bool> IsEmailChangableToAsync(long id, string newEmail);

        public Task<bool?> IsAccountActiveAsync(string email);

        Task<bool> DisableAccountAsync(long id);

        Task<bool> EnableAccountAsync(long id);

        public void UpdateAccountCredentials(Account account);

        public Task<Account> GetAccountCredentialsByEmailAsync(string email);

        public Task<Account> GetAccountByTelegramToken(string token);

        public Task<List<Account>> GetAllAccountsWithTelegramTokens();

        public Task<Account> GetAccountByTelegramId(long telegramId);

        public Task<Account> GetAccountByTelegramId(string telegramId);
    }
}
