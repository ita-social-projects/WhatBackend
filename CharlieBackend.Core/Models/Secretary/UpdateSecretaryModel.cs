using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.Secretary
{
    class UpdateSecretaryModel : SecretaryModel
    {
        [Required]
        [JsonIgnore]
        public long Id { get; set; }

        public new string Email { get; set; }

        public string Password
        {
            get => base.Password;
            set => base.Password = value;
        }

        [JsonPropertyName("first_name")]
        [StringLength(30)]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        [StringLength(30)]
        public string LastName { get; set; }
    }
}
