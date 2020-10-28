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
            catch
            {
                _unitOfWork.Rollback();

                return null;
            }
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

        public Task<bool?> IsAccountActiveAsync(string email)
        {
            return _unitOfWork.AccountRepository.IsAccountActiveAsync(email);
        }

        public async Task<bool> DisableAccountAsync(long id)
        {
            try
            {
                var isSucceeded = await _unitOfWork.AccountRepository.DisableAccountAsync(id);

                await _unitOfWork.CommitAsync();

                return true;
            }
            catch
            {
                _unitOfWork.Rollback();

                return false;
            }
        }

        #region hash
        public string GenerateSalt()
        {
            //StringBuilder object with a predefined buffer size for the resulting string
            StringBuilder sb = new StringBuilder(_saltLen - 1);

            //a variable for storing a random character position from the string Str
            int Position = 0;

            for (int i = 0; i < _saltLen; i++)
            {
                Position = this.Next(0, _saltAlphabet.Length - 1);

                //add the selected character to the object StringBuilder
                sb.Append(_saltAlphabet[Position]);
            }

            return sb.ToString();
        }
        public Int32 Next(Int32 minValue, Int32 maxValue)
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uint32Buffer = new byte[4];
                Int64 diff = maxValue - minValue;
                while (true)
                {
                    rng.GetBytes(uint32Buffer);
                    UInt32 rand = BitConverter.ToUInt32(uint32Buffer, 0);
                    Int64 max = (1 + (Int64)UInt32.MaxValue);
                    Int64 remainder = max % diff;
                    if (rand < max - remainder)
                    {
                        return (Int32)(minValue + (rand % diff));
                    }
                }
            }
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
