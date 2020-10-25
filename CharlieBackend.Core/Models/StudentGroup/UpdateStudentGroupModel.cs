using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.StudentGroup
{
    public class UpdateStudentGroupModel : StudentGroupModel
    {
        [Required]
        [JsonIgnore]
        public override long Id { get; set; }

        [Required]
        [JsonPropertyName("student_ids")]
        public new List<long> StudentIds { get; set; }

        [Required]
        [JsonPropertyName("mentor_ids")]
        public new List<long> MentorIds { get; set; }

        [Required]
        [JsonPropertyName("course_id")]
        public new long CourseId { get; set; }
    }
}
