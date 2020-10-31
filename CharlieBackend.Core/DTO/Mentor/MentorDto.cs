using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Mentor
{
    public class MentorDto
    {
        [Required]
        public long Id { get; set; }

        [JsonIgnore]
        [StringLength(65)]
        public string Password { get; set; }


        [JsonIgnore]
        public int Role { get; set; }

        [Required]
        [JsonIgnore]
        [JsonPropertyName("course_ids")]
        public List<long> CourseIds { get; set; }

        [Required]
        [JsonIgnore]
        public bool IsActive { get; set; }
        
    }
}
