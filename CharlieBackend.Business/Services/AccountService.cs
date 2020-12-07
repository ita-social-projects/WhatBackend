using System;
using AutoMapper;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
//using System.Security.Cryptography;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Business.Providers;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Data.Repositories.Impl.Interfaces;

namespace CharlieBackend.Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationService _notification;

        public AccountService(IUnitOfWork unitOfWork, 
                              IMapper mapper,
                              INotificationService notification)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notification = notification;
        }

        public async Task<Result<AccountDto>> CreateAccountAsync(CreateAccountDto accountModel)
        {
            var isEmailTaken = await IsEmailTakenAsync(accountModel.Email);

            if (isEmailTaken)
            {
                return Result<AccountDto>.GetError(ErrorCode.Conflict,
                    "Account already exists!");
            }

            try
            {
                var account = new Account
                {
                    Email = accountModel.Email,
                    FirstName = accountModel.FirstName,
                    LastName = accountModel.LastName
                };

                account.Salt = HashPasswordProvider.GenerateSalt();
                account.Password = HashPasswordProvider.HashPassword(accountModel.ConfirmPassword, account.Salt);

                _unitOfWork.AccountRepository.Add(account);

                await _unitOfWork.CommitAsync();

                await _notification.RegistrationSuccess(account);

                return Result<AccountDto>.GetSuccess(_mapper.Map<AccountDto>(account));
            }
            catch
            {
                _unitOfWork.Rollback();

                return Result<AccountDto>.GetError(ErrorCode.InternalServerError, "Cannot create account.");
            }
        }

        public async Task<AccountDto> GetAccountCredentialsAsync(AuthenticationDto authenticationModel)
        {
            var salt = await _unitOfWork.AccountRepository.GetAccountSaltByEmail(authenticationModel.Email);

            if (salt != "")
            {
                authenticationModel.Password = HashPasswordProvider.HashPassword(authenticationModel.Password, salt);

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

        public async Task<Result<AccountDto>> ChangePasswordAsync(ChangeCurrentPasswordDto changePasswd)
        {
            var user = await _unitOfWork.AccountRepository.GetAccountCredentialsByEmailAsync(changePasswd.Email);
            
            if (user == null)
            {
                return Result<AccountDto>.GetError(ErrorCode.NotFound, "Account does not exist.");
            }

            var salt = await _unitOfWork.AccountRepository.GetAccountSaltByEmail(changePasswd.Email);

            if (!string.IsNullOrEmpty(salt))
            {
                string checkPassword = HashPasswordProvider.HashPassword(changePasswd.CurrentPassword, salt);

                if (user.Password == checkPassword)
                {
                    user.Salt = HashPasswordProvider.GenerateSalt();
                    user.Password = HashPasswordProvider.HashPassword(changePasswd.NewPassword, user.Salt);

                    await _unitOfWork.CommitAsync();

                    return Result<AccountDto>.GetSuccess(_mapper.Map<AccountDto>(user));
                }
                else
                {
                    return Result<AccountDto>.GetError(ErrorCode.Conflict, "Wrong current password.");
                }
            }

            return Result<AccountDto>.GetError(ErrorCode.InternalServerError, "Salt for this account does not exist.");
        }
    }
}
