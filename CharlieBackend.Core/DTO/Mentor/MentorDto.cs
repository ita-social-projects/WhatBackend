using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Mentor
{
    public class MentorDto
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [JsonPropertyName("first_name")]
        [StringLength(30)]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        [StringLength(30)]
        public string LastName { get; set; }

        [JsonIgnore]
        [StringLength(65)]
        public string Password { get; set; }

        [JsonIgnore]
        [EnumDataType(typeof(Roles))]
        public Roles Role { get; set; }

        [Required]
        [JsonPropertyName("course_ids")]
        public List<long> CourseIds { get; set; }

        [Required]
        [JsonIgnore]
        public bool IsActive { get; set; }
        
    }
}
