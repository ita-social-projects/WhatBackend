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

        [RegularExpression(@"^(1|2|4)$", ErrorMessage = "Set value 1, 2 or 4 in Role property")]
        public int Role { get; set; }
    }
}
