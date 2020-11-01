using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using CharlieBackend.Core.Entities;


namespace CharlieBackend.Core.DTO.Account
{
    public class AccountDto
    {
        [Required]
        public long Id { get; set; }

        [JsonPropertyName("first_name")]
        [StringLength(30)]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        [StringLength(30)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }


        [StringLength(65)]
        public string Password { get; set; }

        [EnumDataType(typeof(Roles))]
        public Roles Role { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
