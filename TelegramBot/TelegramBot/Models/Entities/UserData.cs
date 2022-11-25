using CharlieBackend.Core.Entities;
using System;

namespace TelegramBot.Models.Entities
{
    [Serializable]
    public class UserData
    {
        public long AccountId { get; set; }
        public long EntityId { get; set; }
        public UserRole CurrentRole { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccessToken { get; set; }
        public string Email { get; set; }
        public string Localization { get; set; }
        public DateTime? Created { get; set; }
    }
}
