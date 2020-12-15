using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Account
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [Url]
        [StringLength(200)]
        public string FormUrl { get; set; }
    }
}
