using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Account
{
    public class ChangeCurrentPasswordDto
    {
        public string Email { get; set; }

        public string CurrentPassword { get; set; } 

        public string NewPassword { get; set; }

        public string ConfirmNewPassword { get; set; }
    }
}
