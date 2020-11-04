using System;
using System.Text;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Lesson
{
    public class StudentLessonDto
    {
        [Required]
        [JsonPropertyName("theme_name")]
        [StringLength(100)]
        public string ThemeName { get; set; }

        [Required]
        public long Id { get; set; }

        [Required]
        public bool Presence { get; set; }

        public sbyte? Mark { get; set; }

        [StringLength(1024)]
        public string Comment { get; set; }

        [JsonPropertyName("student_group_id")]
        public long? StudentGroupId { get; set; }

        [JsonPropertyName("lesson_date")]
        [DataType(DataType.DateTime)]
        public DateTime LessonDate { get; set; }
    }
}
