using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Models;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AccountModel> CreateAccountAsync(AccountModel accountModel)
        {
            try
            {
                // TODO: hash password, add salt, add role
                await _unitOfWork.AccountRepository.PostAsync(accountModel.ToAccount());
                _unitOfWork.Commit();
                return accountModel;

            } catch { _unitOfWork.Rollback(); return null; }
        }

        public async Task<AccountModel> GetAccountCredentials(AuthenticationModel authenticationModel)
        {
            var foundAccount = await _unitOfWork.AccountRepository.GetAccountCredentials(authenticationModel);
            return foundAccount?.ToAccountModel();
        }
    }
}
