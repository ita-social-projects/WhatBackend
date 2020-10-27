using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models
{
    public class BaseAccountModel
    {
        [Required]
        public virtual long Id { get; set; }

        [JsonPropertyName("first_name")]
        [StringLength(30)]
        public virtual string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        [StringLength(30)]
        public virtual string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public virtual string Email { get; set; }

        
        [StringLength(65)]
        public virtual string Password { get; set; }

        //[RegularExpression(@"^(1|2|4)$", ErrorMessage = "Set value 1, 2 or 4 in Role property")] //not mine comment
        public virtual int Role { get; set; }
        
        [Required]
        public virtual bool IsActive { get; set; }
    }
}
