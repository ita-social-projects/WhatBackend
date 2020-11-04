using System;
using System.Text;
using System.Collections.Generic;
using CharlieBackend.Core.DTO.Visit;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Lesson
{
    public class UpdateLessonDto
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [JsonPropertyName("theme_name")]
        [StringLength(100)]
        public string ThemeName { get; set; }

        [JsonPropertyName("lesson_date")]
        [DataType(DataType.DateTime)]
        public DateTime LessonDate { get; set; }

        [Required]
        [JsonPropertyName("lesson_visits")]
        public IList<VisitDto> LessonVisits { get; set; }
    }
}
