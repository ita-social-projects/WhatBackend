using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Account
{
    public class AuthenticationDto
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
