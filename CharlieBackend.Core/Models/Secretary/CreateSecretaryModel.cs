using System;
using System.Text;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.Secretary
{
    public class CreateSecretaryModel : SecretaryModel
    {
        [Required]
        [JsonIgnore]
        public long Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [Required] //can be null in bd
        [JsonPropertyName("first_name")]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required] //can be null in bd
        [JsonPropertyName("last_name")]
        [StringLength(30)]
        public string LastName { get; set; }
    }
}
