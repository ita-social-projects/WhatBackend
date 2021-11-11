using AutoMapper;
using CharlieBackend.Business.Helpers;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Extensions;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationService _notification;
        private readonly ICurrentUserService _currentUserService;

        public AccountService(IUnitOfWork unitOfWork,
                              IMapper mapper,
                              INotificationService notification,
                              ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notification = notification;
            _currentUserService = currentUserService;
        }

        public async Task<Result<AccountRoleDto>> GrantRoleToAccount(
                AccountRoleDto accountRole)
        {
            Account user = await _unitOfWork.AccountRepository
                    .GetAccountCredentialsByEmailAsync(accountRole.Email);

            user.UpdatedByAccountId = _currentUserService.AccountId;

            Result<AccountRoleDto> result = null;

            if (user == null)
            {
                result = Result<AccountRoleDto>.GetError(ErrorCode.NotFound,
                        "Account not found");
            }
            else
            {
                if (await user.GrantAccountRoleAsync(accountRole.Role))
                {
                    await AddGrantedRoleToRepositoryAsync(accountRole, user);

                    await _unitOfWork.CommitAsync();

                    result = Result<AccountRoleDto>.GetSuccess(
                            _mapper.Map<AccountRoleDto>(user));
                }
                else
                {
                    result = Result<AccountRoleDto>.GetError(
                            ErrorCode.Conflict, "Account allready has" +
                            " this role or role is unsuitable");
                }
            }
         
            return result;
        }

        private async Task AddGrantedRoleToRepositoryAsync(AccountRoleDto accountRole,
                Account user)
        {
            switch (accountRole.Role)
            {
                case UserRole.Student:
                    if (await _unitOfWork.StudentRepository
                            .GetStudentByAccountIdAsync(user.Id) == null)
                    {
                        Student newStudent = new Student()
                        {
                            Account = user,
                            AccountId = user.Id,
                        };

                        _unitOfWork.StudentRepository.Add(newStudent);
                    }
                    break;

                case UserRole.Mentor:
                    if (await _unitOfWork.MentorRepository
                            .GetMentorByAccountIdAsync(user.Id) == null)
                    {
                        Mentor newMentor = new Mentor()
                        {
                            Account = user,
                            AccountId = user.Id
                        };

                        _unitOfWork.MentorRepository.Add(newMentor);
                    }
                    break;

                case UserRole.Secretary:
                    if (await _unitOfWork.SecretaryRepository
                            .GetSecretaryByAccountIdAsync(user.Id) == null)
                    {
                        Secretary newSecretary = new Secretary()
                        {
                            Account = user,
                            AccountId = user.Id
                        };

                        _unitOfWork.SecretaryRepository.Add(newSecretary);
                    }
                    break;

                default:
                    break;
            }
        }

        public async Task<Result<AccountRoleDto>> RevokeRoleFromAccount(
                AccountRoleDto accountRole)
        {
            Account user = await _unitOfWork.AccountRepository
                    .GetAccountCredentialsByEmailAsync(accountRole.Email);

            user.UpdatedByAccountId = _currentUserService.AccountId;

            Result<AccountRoleDto> result = null;

            if (user == null)
            {
                result = Result<AccountRoleDto>.GetError(ErrorCode.NotFound,
                        "Account not found");
            }
            else
            {           
                if (await user.RevokeAccountRoleAsync(accountRole.Role))
                {
                    await _unitOfWork.CommitAsync();

                    result = Result<AccountRoleDto>.GetSuccess(
                            _mapper.Map<AccountRoleDto>(user));
                }
                else
                {
                    result = Result<AccountRoleDto>.GetError(ErrorCode.Conflict,
                            "Account doesn't have this role or role is unsuitable");
                }
            }

            return result;
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

                account.Salt = PasswordHelper.GenerateSalt();
                account.Password = PasswordHelper.HashPassword(accountModel.ConfirmPassword, account.Salt);

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

        public async Task<Result<AccountDto>> GetAccountCredentialsAsync(AuthenticationDto authenticationModel)
        {
            var account = await _unitOfWork.AccountRepository.GetAccountCredentialsByEmailAsync(authenticationModel.Email);

            if (account != null)
            {
                string password = PasswordHelper.HashPassword(authenticationModel.Password, account.Salt);

                if (password != account.Password)
                {
                    return Result<AccountDto>.GetError(ErrorCode.Unauthorized, "Email or password is incorrect.");
                }
                else
                {
                    var foundAccount = _mapper.Map<AccountDto>(account);

                    return Result<AccountDto>.GetSuccess(foundAccount);
                }
            }

            return Result<AccountDto>.GetError(ErrorCode.NotFound, "User Not Found");

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
            var isSucceeded = await _unitOfWork.AccountRepository.DisableAccountAsync(id, _currentUserService.AccountId);

            await _unitOfWork.CommitAsync();

            return isSucceeded;
        }

        public async Task<bool> EnableAccountAsync(long id)
        {
            var isSucceeded = await _unitOfWork.AccountRepository.EnableAccountAsync(id, _currentUserService.AccountId);

            await _unitOfWork.CommitAsync();

            return isSucceeded;
        }

        public async Task<Result<AccountDto>> ChangePasswordAsync(ChangeCurrentPasswordDto changePassword)
        {
            var email = _currentUserService.Email;
            var user = await _unitOfWork.AccountRepository.GetAccountCredentialsByEmailAsync(email);

            if (user == null)
            {
                return Result<AccountDto>.GetError(ErrorCode.NotFound, "Account does not exist.");
            }

            var salt = user.Salt;

            if (!string.IsNullOrEmpty(salt))
            {
                string checkPassword = PasswordHelper.HashPassword(changePassword.CurrentPassword, salt);

                if (user.Password == checkPassword)
                {
                    user.Salt = PasswordHelper.GenerateSalt();
                    user.Password = PasswordHelper.HashPassword(changePassword.NewPassword, user.Salt);
                    user.UpdatedByAccountId = _currentUserService.AccountId;

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

        public async Task<Result<ForgotPasswordDto>> GenerateForgotPasswordToken(ForgotPasswordDto forgotPassword)
        {
            var user = await _unitOfWork.AccountRepository.GetAccountCredentialsByEmailAsync(forgotPassword.Email);

            if (user == null)
            {
                return Result<ForgotPasswordDto>.GetError(ErrorCode.NotFound,
                        $"Account with email {forgotPassword.Email} does not exist!");
            }

            user.ForgotPasswordToken = Guid.NewGuid().ToString();
            user.ForgotTokenGenDate = DateTime.UtcNow;

            await _unitOfWork.CommitAsync();

            string callbackUrl = forgotPassword.FormUrl + "?token=" + user.ForgotPasswordToken;

            await _notification.ForgotPasswordNotify(forgotPassword.Email, callbackUrl);

            return Result<ForgotPasswordDto>.GetSuccess(forgotPassword);
        }

        public async Task<Result<AccountDto>> ResetPasswordAsync(string guid, ResetPasswordDto resetPassword)
        {
            var user = await _unitOfWork.AccountRepository.GetAccountCredentialsByEmailAsync(resetPassword.Email);

            if (user == null)
            {
                return Result<AccountDto>.GetError(ErrorCode.NotFound, "Account does not exist.");
            }

            if (user.ForgotPasswordToken != guid)
            {
                return Result<AccountDto>.GetError(ErrorCode.ValidationError, "Invalid forgot password token");
            }

            DateTime tokenGenDate = (DateTime)user.ForgotTokenGenDate;

            if (DateTime.Now > tokenGenDate.AddDays(1) || !user.ForgotTokenGenDate.HasValue)
            {
                return Result<AccountDto>.GetError(ErrorCode.ForgotPasswordExpired,
                                                   $"Forgot password token for {user.Email} is expired");
            }

            user.Salt = PasswordHelper.GenerateSalt();
            user.Password = PasswordHelper.HashPassword(resetPassword.NewPassword, user.Salt);
            user.ForgotPasswordToken = null;
            user.ForgotTokenGenDate = null;

            await _unitOfWork.CommitAsync();

            return Result<AccountDto>.GetSuccess(_mapper.Map<AccountDto>(user));
        }

        public async Task<Result<AccountDto>> GetAccountCredentialsByEmailAsync(
                string email)
        {
            var account = await _unitOfWork.AccountRepository
                    .GetAccountCredentialsByEmailAsync(email);

            if (account != null)
            {
                return Result<AccountDto>.GetSuccess(_mapper.Map<AccountDto>(account));
            }

            return Result<AccountDto>.GetError(ErrorCode.NotFound,
                    "Account with this email not found");
        }
    }
}
