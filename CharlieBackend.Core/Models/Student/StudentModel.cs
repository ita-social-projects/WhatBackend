using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.Student
{
    public class StudentModel
    {
        
        [Required]
        [JsonIgnore]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [JsonIgnore]
        [StringLength(65)]
        public string Password { get; set; }

        [JsonIgnore]
        public int Role { get; set; }

        [Required]
        [JsonIgnore]
        public bool IsActive { get; set; }

        [JsonIgnore]
        [JsonPropertyName("student_group_ids")]
        public virtual List<long> StudentGroupIds { get; set; }

    }
}
