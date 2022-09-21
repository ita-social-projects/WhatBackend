using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.ResultModel;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface ITelegramService
    {
        Task<Result<string>> GetTelegramBotLink();

        Task<Result<Account>> SynchronizeTelegramAccount(string telegramToken, string telegramId);

        Task<bool> ClearOldTelegramTokens();

        Task<Account> GetAccountByTelegramId(long telegramId);
    }
}
