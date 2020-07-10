using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models;
using CharlieBackend.Core.Models.Account;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IAccountService
    {
        Task<BaseAccountModel> CreateAccountAsync(BaseAccountModel accountModel);
        Task<BaseAccountModel> GetAccountCredentialsAsync(AuthenticationModel authenticationModel);
        Task<BaseAccountModel> UpdateAccountCredentialsAsync(Account account);
        Task<bool> IsEmailTakenAsync(string email);
        Task<bool> IsEmailChangableToAsync(string newEmail);
        Task<bool> IsAccountActiveAsync(string email);
        Task<bool> DisableAccountAsync(long id);
        public string GenerateSalt();
        public string HashPassword(string password, string salt);
    }
}
