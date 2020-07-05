using CharlieBackend.Core.Models;
using CharlieBackend.Core.Models.Account;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IAccountService
    {
        Task<BaseAccountModel> CreateAccountAsync(BaseAccountModel accountModel);
        Task<BaseAccountModel> GetAccountCredentialsAsync(AuthenticationModel authenticationModel);
        Task<bool> IsEmailTakenAsync(string email);
    }
}
