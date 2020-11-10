using System;
using System.Text;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using CharlieBackend.Core.Entities;

namespace CharlieBackend.Core.Models.Secretary
{
    public class SecretaryModel
    {
        [JsonIgnore]
        [StringLength(65)]
        public string Password { get; set; }
        
        [JsonIgnore]
        public UserRole Role { get; set; }

        [Required]
        [JsonIgnore]
        public bool IsActive { get; set; }
    }
}
