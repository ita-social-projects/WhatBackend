using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Secretary
{
    public class CreateSecretaryDto
    {
        public long Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [Required] //can be null in bd
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required] //can be null in bd
        [StringLength(30)]
        public string LastName { get; set; }
    }
}
