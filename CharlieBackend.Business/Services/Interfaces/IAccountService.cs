using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.Models.ResultModel;


namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IAccountService
    {
        Task<Result<AccountDto>> CreateAccountAsync(CreateAccountDto accountModel);

        Task<AccountDto> GetAccountCredentialsAsync(AuthenticationDto authenticationModel);

        Task<Account> GetAccountCredentialsByIdAsync(long id);

        Task<IList<AccountDto>> GetAllAccountsAsync();

        Task<IList<AccountDto>> GetAllNotAssignedAccountsAsync();

        //Task<AccountDto> UpdateAccountCredentialsAsync(Account account);

        Task<bool> IsEmailTakenAsync(string email);

        Task<bool> IsEmailChangableToAsync(long id, string newEmail);

        Task<bool?> IsAccountActiveAsync(string email);

        Task<bool> DisableAccountAsync(long id);

        public string GenerateSalt();

        public string HashPassword(string password, string salt);

       Task<Result<AccountDto>> ChangePasswordAsync(ChangeCurrentPasswordDto changePasswd);
    }
}
