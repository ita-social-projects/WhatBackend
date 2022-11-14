using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TelegramBot.Models.Entities;
using TelegramBot.Services.Interfaces;

namespace TelegramBot.Services
{
    public class UserDataService : IUserDataService
    {
        public Dictionary<long, UserData> UserDataByTelegramId { get; }

        private readonly BinaryFormatter _binaryFormatter;

        public UserDataService()
        {
            _binaryFormatter = new BinaryFormatter();
            //ToDo: deserialize user data from file to dictionary or if the file not exist initialize the new one
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
        }

        public void AddUserData(long telegramId, UserData userData)
        {
            UserDataByTelegramId.Add(telegramId, userData);

            //Todo: add Serialization of UserDataByTelegramId here
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
    }
}
