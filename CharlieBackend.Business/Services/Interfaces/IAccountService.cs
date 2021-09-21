﻿using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.Models.ResultModel;


namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IAccountService
    {
        Task<Result<AccountDto>> CreateAccountAsync(CreateAccountDto accountModel);

        Task<Result<AccountDto>> GetAccountCredentialsAsync(AuthenticationDto authenticationModel);

        Task<Account> GetAccountCredentialsByIdAsync(long id);

        Task<IList<AccountDto>> GetAllAccountsAsync();

        Task<IList<AccountDto>> GetAllNotAssignedAccountsAsync();

        Task<bool> IsEmailTakenAsync(string email);

        Task<Result<AccountDto>> GetAccountCredentialsByEmailAsync(string email);

        Task<bool> IsEmailChangableToAsync(long id, string newEmail);

        Task<bool?> IsAccountActiveAsync(string email);

        Task<bool> DisableAccountAsync(long id);

        Task<bool> EnableAccountAsync(long id);

        Task<Result<AccountDto>> ChangePasswordAsync(ChangeCurrentPasswordDto changePasswd);

        Task<Result<ForgotPasswordDto>> GenerateForgotPasswordToken(ForgotPasswordDto forgotPassword);

        Task<Result<AccountDto>> ResetPasswordAsync(string guid, ResetPasswordDto resetPassword);

        Task<Result<AccountRoleDto>> GrantRoleToAccount(AccountRoleDto accountRole);

        Task<Result<AccountRoleDto>> RevokeRoleFromAccount(AccountRoleDto accountRole);

        Task<Result<string>> GetTelegramBotLink();

        Task<Result<Account>> SynchronizeTelegramAccount(string telegramToken, string telegramId);

        Task<bool> ClearOldTelegramTokens();
        Task<Account> GetAccountByTelegramId(long telegramId);
    }
}
