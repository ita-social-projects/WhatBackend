using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.DTO.StudentGroups
{
    public class UpdateStudentGroupDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [JsonPropertyName("course_id")]
        public long CourseId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [JsonPropertyName("start_date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [JsonPropertyName("finish_date")]
        public DateTime FinishDate { get; set; }

        [Required]
        [JsonPropertyName("student_ids")]
        public IList<long> StudentIds { get; set; }

        [Required]
        [JsonPropertyName("mentor_ids")]
        public IList<long> MentorIds { get; set; }
    }
}