using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models;
using CharlieBackend.Core.DTO.Account;
using System.Threading.Tasks;
using CharlieBackend.Core.Models.ResultModel;
using System.Collections.Generic;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IAccountService
    {
        Task<Result<AccountDto>> CreateAccountAsync(CreateAccountDto accountModel);

        Task<AccountDto> GetAccountCredentialsAsync(AuthenticationDto authenticationModel);

        Task<Account> GetAccountCredentialsByIdAsync(long id);

        Task<IList<AccountDto>> GetAllAccountsAsync();

        Task<AccountDto> UpdateAccountCredentialsAsync(Account account);

        Task<bool> IsEmailTakenAsync(string email);

        Task<bool> IsEmailChangableToAsync(string newEmail);

        Task<bool?> IsAccountActiveAsync(string email);

        Task<bool> DisableAccountAsync(long id);

        public string GenerateSalt();

        public string HashPassword(string password, string salt);
    }
}
