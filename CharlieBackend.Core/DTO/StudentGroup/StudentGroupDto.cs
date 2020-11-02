using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.DTO.StudentGroups
{
    public class StudentGroupDto
    {
        public long Id { get; set; }

        [Required]
        [JsonPropertyName("course_id")]
        public virtual long? CourseId { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [JsonPropertyName("start_date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [JsonPropertyName("finish_date")]
        public DateTime FinishDate { get; set; }

        [JsonPropertyName("student_ids")]
        public virtual IList<long> StudentIds { get; set; }

        [JsonPropertyName("mentor_ids")]
        public virtual IList<long> MentorIds { get; set; }

    }
}