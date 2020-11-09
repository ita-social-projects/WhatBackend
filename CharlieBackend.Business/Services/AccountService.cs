﻿using CharlieBackend.Business.Services.Interfaces;
using AutoMapper;
using CharlieBackend.Core;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CharlieBackend.Core.Models.ResultModel;

namespace CharlieBackend.Business.Services
{
    public class AccountService : IAccountService
    {
        private const string _saltAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-01234567890";
        private const int _saltLen = 15;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICredentialsSenderService _credentialsSender;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper, ICredentialsSenderService credentialsSender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _credentialsSender = credentialsSender;
        }

        public async Task<Result<AccountDto>> CreateAccountAsync(CreateAccountDto accountModel)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var account = new Account
                    {
                        Email = accountModel.Email,
                        FirstName = accountModel.FirstName,
                        LastName = accountModel.LastName,
                        Role = 0
                    };

                    account.Salt = GenerateSalt();
                    account.Password = HashPassword(accountModel.ConfirmPassword, account.Salt);

                    _unitOfWork.AccountRepository.Add(account);

                    await _unitOfWork.CommitAsync();

                    if (await _credentialsSender.SendCredentialsAsync(account.Email, accountModel.ConfirmPassword))
                    {
                        transaction.Commit();

                        return Result<AccountDto>.Success(_mapper.Map<AccountDto>(account));
                    }
                    else
                    {
                        transaction.Commit();

                        return Result<AccountDto>.Success(_mapper.Map<AccountDto>(account));
                    }
                }
                catch
                {
                    transaction.Rollback();

                    return Result<AccountDto>.Error(ErrorCode.InternalServerError, "Cannot create account.");
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

        public async Task<Account> GetAccountCredentialsByIdAsync(long id)
        {
            var account = await _unitOfWork.AccountRepository.GetAccountCredentialsById(id);

            if (account != null)
            {
                return account;
            }

            return null;
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
