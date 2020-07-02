using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Models;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Threading.Tasks;
using System;
using System.Text;
using System.Security.Cryptography;

namespace CharlieBackend.Business.Services
{
    public class AccountService : IAccountService
    {
        private const string _saltAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-01234567890";
        private const int _saltLen = 15;

        private readonly IUnitOfWork _unitOfWork;
        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AccountModel> CreateAccountAsync(AccountModel accountModel)
        {
            try
            {
                // TODO: add role
                var account = accountModel.ToAccount();
                account.Salt = GenerateSalt();
                account.Password = HashPassword(account.Password + account.Salt);

                _unitOfWork.AccountRepository.Add(account);
                await _unitOfWork.CommitAsync();
                return account.ToAccountModel();
            }
            catch { _unitOfWork.Rollback(); return null; }
        }

        public async Task<AccountModel> GetAccountCredentialsAsync(AuthenticationModel authenticationModel)
        {
            var salt = await _unitOfWork.AccountRepository.GetAccountSalt(authenticationModel.email);
            if (salt != "")
            {
                authenticationModel.password = HashPassword(authenticationModel.password + salt);
                var foundAccount = await _unitOfWork.AccountRepository.GetAccountCredentials(authenticationModel);
                return foundAccount?.ToAccountModel();
            }
            return null;
        }

        public Task<bool> IsEmailTakenAsync(string email)
        {
            return _unitOfWork.AccountRepository.IsEmailTakenAsync(email);
        }

        #region hash
        private string GenerateSalt()
        {
            //create a random object that generates random numbers
            Random rand = new Random();
            //StringBuilder object with a predefined buffer size for the resulting string
            StringBuilder sb = new StringBuilder(_saltLen - 1);
            //a variable for storing a random character position from the string Str
            int Position = 0;

            for (int i = 0; i < _saltLen; i++)
            {
                Position = rand.Next(0, _saltAlphabet.Length - 1);
                //add the selected character to the object StringBuilder
                sb.Append(_saltAlphabet[Position]);
            }

            return sb.ToString();
        }
        private string HashPassword(string text)
        {
            byte[] data = Encoding.Default.GetBytes(text);
            var result = new SHA256Managed().ComputeHash(data);
            return BitConverter.ToString(result).Replace("-", "").ToLower();
        }

        #endregion
    }
}
