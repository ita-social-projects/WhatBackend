using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.Account
{
    public class AuthenticationModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
