using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models;
using CharlieBackend.Core.Models.Account;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<BaseAccountModel> CreateAccountAsync(BaseAccountModel accountModel)
        {
            try
            {
                // TODO: add role
                var account = accountModel.ToAccount();
                account.Salt = GenerateSalt();
                account.Password = HashPassword(account.Password, account.Salt);

                _unitOfWork.AccountRepository.Add(account);
                await _unitOfWork.CommitAsync();
                return account.ToAccountModel();
            }
            catch { _unitOfWork.Rollback(); return null; }
        }

        public async Task<BaseAccountModel> GetAccountCredentialsAsync(AuthenticationModel authenticationModel)
        {
            var salt = await _unitOfWork.AccountRepository.GetAccountSaltByEmail(authenticationModel.Email);
            if (salt != "")
            {
                authenticationModel.Password = HashPassword(authenticationModel.Password, salt);
                var foundAccount = await _unitOfWork.AccountRepository.GetAccountCredentials(authenticationModel);
                return foundAccount?.ToAccountModel();
            }
            return null;
        }

        public Task<bool> IsEmailTakenAsync(string email)
        {
            return _unitOfWork.AccountRepository.IsEmailTakenAsync(email);
        }

        public async Task<BaseAccountModel> UpdateAccountCredentialsAsync(Account account)
        {
            account.Salt = GenerateSalt();
            account.Password = HashPassword(account.Password, account.Salt);

            _unitOfWork.AccountRepository.UpdateAccountCredentials(account);
            await _unitOfWork.CommitAsync();

            return account.ToAccountModel();
        }

        public Task<bool> IsEmailChangableToAsync(string newEmail)
        {
            return _unitOfWork.AccountRepository.IsEmailChangableToAsync(newEmail);
        }

        #region hash
        public string GenerateSalt()
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
        public string HashPassword(string password, string salt)
        {
            byte[] data = Encoding.Default.GetBytes(password + salt);
            var result = new SHA256Managed().ComputeHash(data);
            return BitConverter.ToString(result).Replace("-", "").ToLower();
        }

        #endregion
    }
}
