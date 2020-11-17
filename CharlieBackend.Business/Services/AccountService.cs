using System;
using AutoMapper;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using System.Security.Cryptography;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Data.Repositories.Impl.Interfaces;

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

        public async Task<Result<AccountDto>> CreateAccountAsync(CreateAccountDto accountModel)
        {
            var isEmailTaken = await IsEmailTakenAsync(accountModel.Email);

            if (isEmailTaken)
            {
                return Result<AccountDto>.GetError(ErrorCode.Conflict,
                    "Account already exists!");
            }

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var account = new Account
                    {
                        Email = accountModel.Email,
                        FirstName = accountModel.FirstName,
                        LastName = accountModel.LastName
                    };

                    account.Salt = GenerateSalt();
                    account.Password = HashPassword(accountModel.ConfirmPassword, account.Salt);

                    _unitOfWork.AccountRepository.Add(account);

                    await _unitOfWork.CommitAsync();

                    transaction.Commit();

                    return Result<AccountDto>.GetSuccess(_mapper.Map<AccountDto>(account));
                   
                }
                catch
                {
                    transaction.Rollback();

                    return Result<AccountDto>.GetError(ErrorCode.InternalServerError, "Cannot create account.");
                }
            }
        }

        public async Task<AccountDto> GetAccountCredentialsAsync(AuthenticationDto authenticationModel)
        {
            var salt = await _unitOfWork.AccountRepository.GetAccountSaltByEmail(authenticationModel.Email);

            if (salt != "")
            {

                authenticationModel.Password = HashPassword(authenticationModel.Password, salt);

                var foundAccount = _mapper.Map<AccountDto>(await _unitOfWork.AccountRepository.GetAccountCredentials(authenticationModel));

                return foundAccount;
            }

            return null;
        }

        public async Task<IList<AccountDto>> GetAllAccountsAsync()
        {
            var foundAccount = _mapper.Map<IList<AccountDto>>(await _unitOfWork.AccountRepository.GetAllAsync());

            return foundAccount;
        }

        public async Task<Account> GetAccountCredentialsByIdAsync(long id)
        {
            var account = await _unitOfWork.AccountRepository.GetAccountCredentialsById(id);

            if (account != null)
            {
                return account;
            }

            return null;
        }

        public async Task<IList<AccountDto>> GetAllNotAssignedAccountsAsync()
        {
            var accounts = _mapper.Map<List<AccountDto>>(await _unitOfWork.AccountRepository.GetAllNotAssignedAsync());

            return accounts;
        }

        public Task<bool> IsEmailTakenAsync(string email)
        {
            return _unitOfWork.AccountRepository.IsEmailTakenAsync(email);
        }

        public async Task<AccountDto> UpdateAccountCredentialsAsync(Account account)
        {
            
            account.Salt = GenerateSalt();
            account.Password = HashPassword(account.Password, account.Salt);

            _unitOfWork.AccountRepository.UpdateAccountCredentials(account);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<AccountDto>(account);
        }

        public Task<bool> IsEmailChangableToAsync(long id, string newEmail)
        {
            return _unitOfWork.AccountRepository.IsEmailChangableToAsync(id, newEmail);
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
