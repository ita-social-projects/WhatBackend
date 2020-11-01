using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Student
{
    public class StudentDto
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [JsonIgnore]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [JsonIgnore]
        [StringLength(65)]
        public string Password { get; set; }

        [JsonIgnore]
        [EnumDataType(typeof(Roles))]
        public Roles Role { get; set; }

        [Required]
        [JsonIgnore]
        public bool IsActive { get; set; }

        [JsonIgnore]
        [JsonPropertyName("student_group_ids")]
        public virtual List<long> StudentGroupIds { get; set; }
    }
}
