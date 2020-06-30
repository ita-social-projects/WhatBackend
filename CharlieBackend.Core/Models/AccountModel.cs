using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models
{
    public class AccountModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public sbyte? Role { get; set; }
    }
}
