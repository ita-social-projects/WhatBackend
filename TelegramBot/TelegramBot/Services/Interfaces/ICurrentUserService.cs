using CharlieBackend.Core.Entities;

namespace TelegramBot.Services.Interfaces
{
    public interface ICurrentUserService
    {
        public long AccountId { get; }
        public long EntityId { get; }
        public string Email { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public UserRole Role { get; }
        public string Localization { get; set; }
    }
}
