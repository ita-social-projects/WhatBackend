using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TelegramBot.Models.Entities;
using TelegramBot.Services.Interfaces;
using System.Linq;
using System;

namespace TelegramBot.Services
{
    public class UserDataService : IUserDataService
    {
        public Dictionary<long, UserData> UserDataByTelegramId { get; }

        private readonly BinaryFormatter _binaryFormatter;

        public UserDataService()
        {
            _binaryFormatter = new BinaryFormatter();

            using (var fs = new FileStream("UserData.dat", FileMode.OpenOrCreate))
            {
                if (fs.Length > 0)
                {
                    UserDataByTelegramId = _binaryFormatter.Deserialize(fs) as Dictionary<long, UserData>;
                }
                else
                {
                    UserDataByTelegramId = new Dictionary<long, UserData>();
                }
            }

            ClearExpiredToken();
        }

        public void AddUserData(long telegramId, UserData userData)
        {
            UserDataByTelegramId[telegramId] = userData;

            using (var fs = new FileStream("UserData.dat", FileMode.OpenOrCreate))
            {
                _binaryFormatter.Serialize(fs, UserDataByTelegramId);
            }
        }

        /// <summary>
        /// returns access token or null if token does not exist
        /// </summary>
        /// <param name="telegramId"></param>
        /// <returns></returns>
        public string GetAccessTokenByTelegramId(long telegramId)
        {
            string token = null;

            UserData userData = null;

            if (UserDataByTelegramId.TryGetValue(telegramId, out userData))
            {
                token = userData.AccessToken;
            }

            return token;
        }

        private void ClearExpiredToken()
        {
            if(UserDataByTelegramId.Any())
            {
                foreach (var user in UserDataByTelegramId)
                {
                    if(user.Value.Created.Value.AddMinutes(720) > DateTime.UtcNow)
                    {
                        UserDataByTelegramId.Remove(user.Key);
                    }
                }
            }
        }
    }
}
