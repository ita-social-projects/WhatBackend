using CharlieBackend.Core.Entities;

namespace CharlieBackend.Business.Services.Interfaces
{
    /// <summary>
    /// Interface that describes accessible properties of the current user.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Property that reflects the account Id of the current user.
        /// </summary>
        public long AccountId { get; }

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
