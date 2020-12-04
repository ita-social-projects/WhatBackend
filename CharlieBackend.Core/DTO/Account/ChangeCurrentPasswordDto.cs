using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Account
{
    public class ChangeCurrentPasswordDto
    {
        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(30)]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } 

        [Required]
        [StringLength(30, ErrorMessage = "Password should has at least 6 symbols.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Password should has at least 6 symbols.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}
