using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models
{
    public class BaseAccountModel
    {
        public virtual long Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }

        [Required]
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }

        //[RegularExpression(@"^(1|2|4)$", ErrorMessage = "Set value 1, 2 or 4 in Role property")]
        public virtual int Role { get; set; }
    }
}
