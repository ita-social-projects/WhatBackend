using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.StudentGroup
{
    public class StudentGroupModel
    {
        [Required]
        public virtual long Id { get; set; }
        
        [StringLength(100)]
        public virtual string Name { get; set; }

        [Required]
        [JsonIgnore]
        [JsonPropertyName("course_id")]
        public virtual long? CourseId { get; set; }

        [RegularExpression(@"^\d{4}-((0\d)|(1[012]))-(([012]\d)|3[01])$")] //TODO
        [JsonPropertyName("start_date")]
        public virtual string StartDate { get; set; }

        [RegularExpression(@"^\d{4}-((0\d)|(1[012]))-(([012]\d)|3[01])$")] //TODO
        [JsonPropertyName("finish_date")]
        public virtual string FinishDate { get; set; }

        [Required]
        [JsonIgnore]
        [JsonPropertyName("student_ids")]
        public virtual List<long> StudentIds { get; set; }

        [Required]
        [JsonIgnore]
        [JsonPropertyName("mentor_ids")]
        public virtual List<long> MentorIds { get; set; }
    }
}
