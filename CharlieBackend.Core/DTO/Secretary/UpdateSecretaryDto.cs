using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Secretary
{
    public class UpdateSecretaryDto
    {

        [JsonIgnore]
        public long Id { get; set; }

        [JsonIgnore]
        [StringLength(65)]
        public string Password { get; set; }

        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [JsonPropertyName("first_name")]
        [StringLength(30)]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        [StringLength(30)]
        public string LastName { get; set; }
    }
}
