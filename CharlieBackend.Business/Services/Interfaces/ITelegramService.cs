using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.ResultModel;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface ITelegramService
    {
        Task<Result<string>> GetTelegramBotLink();

        Task<Result<AccountDto>> SynchronizeTelegramAccount(string telegramToken, string telegramId);

        Task<bool> ClearOldTelegramTokens();

        Task<Result<AccountDto>> GetAccountByTelegramIdAsync(string telegramId);

        //ToDo: need to implement 
        //Task<bool> DeleteAccountSyncronizationAsync(string telegramId);
    }
}
