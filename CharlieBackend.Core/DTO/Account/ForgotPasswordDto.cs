using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Account
{
    public class ForgotPasswordDto
    {
        public string Email { get; set; }

        [Url]
        public string FormUrl { get; set; }
    }
}
