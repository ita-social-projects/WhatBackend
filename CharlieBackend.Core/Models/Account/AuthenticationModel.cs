using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.Account
{
    public class AuthenticationModel
    {
        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(65)]
        public string Password { get; set; }
    }
}
