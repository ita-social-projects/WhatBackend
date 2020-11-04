using System;
using System.Text;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;



namespace CharlieBackend.Core.DTO.Lesson
{
    public class AssignMentorToLessonDto
    {
        [Required]
        [JsonPropertyName("mentor_id")]
        public long MentorId { get; set; }

        [Required]
        [JsonPropertyName("lesson_id")]
        public long LessonId { get; set; }

    }
}
