using CharlieBackend.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IAccountService
    {
        Task<AccountModel> CreateAccountAsync(AccountModel accountModel);
        Task<AccountModel> GetAccountCredentials(AuthenticationModel authenticationModel);
    }
}
