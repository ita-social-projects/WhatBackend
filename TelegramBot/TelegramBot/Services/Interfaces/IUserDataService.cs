using System.Collections.Generic;
using TelegramBot.Models.Entities;

namespace TelegramBot.Services.Interfaces
{
    public interface IUserDataService
    {
        public Dictionary<long, UserData> UserDataByTelegramId { get; }

        public string GetAccessTokenByTelegramId(long telegramId);

        public void AddUserData(long telegramId, UserData userData);
    }
}
