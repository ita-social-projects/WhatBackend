using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Account
{
    public class AuthenticationDto
    {
        [Required(ErrorMessage = "Empty email field")]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Empty password field")]
        [DataType(DataType.Password)]
        [StringLength(65)]
        public string Password { get; set; }
    }
}
