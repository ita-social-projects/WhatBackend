using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System;

namespace CharlieBackend.Core.Models.StudentGroup
{
    public class StudenGroupDto
    {
        [Required]
        public virtual long Id { get; set; }
        
        [StringLength(100)]
        public virtual string Name { get; set; }

        [Required]
        [JsonIgnore]
        [JsonPropertyName("course_id")]
        public virtual long? CourseId { get; set; }

        [DataType(DataType.Date)]
        [JsonPropertyName("start_date")]
        public virtual DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [JsonPropertyName("finish_date")]
        public virtual DateTime FinishDate { get; set; }

        [Required]
        [JsonIgnore]
        [JsonPropertyName("student_ids")]
        public virtual IList<long> StudentIds { get; set; }

        [Required]
        [JsonIgnore]
        [JsonPropertyName("mentor_ids")]
        public virtual IList<long> MentorIds { get; set; }
    }
}
