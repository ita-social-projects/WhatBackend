using AutoMapper;
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
        private readonly IMapper _mapper;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseAccountModel> CreateAccountAsync(BaseAccountModel accountModel)
        {
            try
            {
                // TODO: add role
                //var account = accountModel.ToAccount();

                var accountEntity = _mapper.Map<Account>(accountModel);

                accountEntity.Salt = GenerateSalt();
                accountEntity.Password = HashPassword(accountEntity.Password, accountEntity.Salt);

                _unitOfWork.AccountRepository.Add(accountEntity);

                await _unitOfWork.CommitAsync();

                return _mapper.Map<BaseAccountModel>(accountEntity);
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

                return _mapper.Map<BaseAccountModel>(foundAccount);
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

            return _mapper.Map<BaseAccountModel>(account);
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
