using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models;
using CharlieBackend.Core.DTO.Account;
using System.Threading.Tasks;
using CharlieBackend.Core.Models.ResultModel;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IAccountService
    {
        Task<Result<AccountDto>> CreateAccountAsync(CreateAccountDto accountModel);

        Task<AccountDto> GetAccountCredentialsAsync(AuthenticationDto authenticationModel);

        Task<AccountDto> UpdateAccountCredentialsAsync(Account account);

        Task<bool> IsEmailTakenAsync(string email);

        Task<bool> IsEmailChangableToAsync(string newEmail);

        Task<bool?> IsAccountActiveAsync(string email);

        Task<bool> DisableAccountAsync(long id);

        public string GenerateSalt();

        public string HashPassword(string password, string salt);
    }
}
