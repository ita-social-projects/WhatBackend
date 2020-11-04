using System;
using System.Text;
using System.Collections.Generic;
using CharlieBackend.Core.DTO.Visit;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.DTO.Lesson
{
    public class LessonDto
    {
        public long Id { get; set; }

        [Required]
        [JsonPropertyName("theme_name")]
        [StringLength(100)]
        public string ThemeName { get; set; }

        [JsonPropertyName("mentor_id")]
        public long MentorId { get; set; }

        public long? StudentGroupId { get; set; }

        [JsonPropertyName("lesson_date")]
        [DataType(DataType.DateTime)]
        public DateTime LessonDate { get; set; }

        public virtual IList<VisitDto> Visits { get; set; }
    }
}
