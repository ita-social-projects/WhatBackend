using CharlieBackend.Core.Entities;

namespace CharlieBackend.Panel.Services.Interfaces
{
    public interface ICurrentUserService
    {
        /// <summary>
        /// Property that reflects the account Id of the current user.
        /// </summary>
        public long AccountId { get; }

        /// <summary>
        /// Property that reflects the entity Id of the current user.
        /// </summary>
        public long EntityId { get; }

        /// <summary>
        /// Property that reflects the Email of the current user.
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Property that reflects the role of the current user.
        /// </summary>
        public UserRole Role { get; }
    }
}
