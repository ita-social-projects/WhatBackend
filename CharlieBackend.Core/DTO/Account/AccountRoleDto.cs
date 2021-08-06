using CharlieBackend.Core.Entities;

namespace CharlieBackend.Core.DTO.Account
{
    public class AccountRoleDto
    {
        public string Email { get; set; }

        public UserRole Role { get; set; }
    }
}
