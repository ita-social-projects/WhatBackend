using CharlieBackend.Core.Entities;

namespace CharlieBackend.Core.DTO.Account
{
    public class AccountDto
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public UserRole Role { get; set; }

        public bool IsActive { get; set; }

        public string Localization { get; set; }
    }
}
