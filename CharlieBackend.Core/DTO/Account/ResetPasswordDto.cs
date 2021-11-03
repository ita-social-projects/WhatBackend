namespace CharlieBackend.Core.DTO.Account
{
    public class ResetPasswordDto
    {
        public string Email { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmNewPassword { get; set; }
    }
}
