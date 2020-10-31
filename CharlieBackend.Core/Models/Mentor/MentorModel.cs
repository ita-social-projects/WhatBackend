using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models
{
    public class MentorModel 
    {
        
        [JsonIgnore]
        [StringLength(65)]
        public string Password { get; set; }


        [JsonIgnore]
        public int Role { get; set; }

        [Required]
        [JsonIgnore]
        [JsonPropertyName("course_ids")]
        public virtual List<long> CourseIds { get; set; }

        [Required]
        [JsonIgnore]
        public bool IsActive { get; set; }
    }
}
