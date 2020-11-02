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
        public override long Id { get; set; }

        public new string Email
        {
            get => base.Email;
            set => base.Email = value;
        }

        public override string Password
        {
            get => base.Password;
            set => base.Password = value;
        }

        [JsonPropertyName("first_name")]
        [StringLength(30)]
        public override string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        [StringLength(30)]
        public override string LastName { get; set; }
    }
}
