using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.Mentor
{
    public class UpdateMentorModel : MentorModel
    {
        [Required]
        [JsonIgnore]
        public override long Id { get; set; }

        public new string Password 
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

        [JsonPropertyName("course_ids")]
        public new List<long> CourseIds { get; set; }

        [JsonPropertyName("student_group_ids")]
        public List<long> StudentGroupIds { get; set; }
    }
}
