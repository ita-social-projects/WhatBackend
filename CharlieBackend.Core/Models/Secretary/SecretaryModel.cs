using System;
using System.Text;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.Secretary
{
    public class SecretaryModel : BaseAccountModel
    {
        [JsonIgnore]
        [StringLength(65)]
        public override string Password { get; set; }
        
        [JsonIgnore]
        public override int Role { get; set; }

        [Required]
        [JsonIgnore]
        public override bool IsActive { get; set; }
    }
}
