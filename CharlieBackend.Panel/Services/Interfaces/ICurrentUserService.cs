using CharlieBackend.Core.Entities;

namespace CharlieBackend.Panel.Services.Interfaces
{
    public interface ICurrentUserService
    {
        public long AccountId { get; }
        public long EntityId { get; }
        public string Email { get; }
        public UserRole Role { get; }
    }
}
