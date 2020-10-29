using System.Collections.Generic;
using System.Text.Json.Serialization;
using CharlieBackend.Core.Models.Visit;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.Lesson
{
    public class AssignMentorToLessonModel
    {

        [Required]
        [JsonPropertyName("mentor_id")]
        public long MentorId { get; set; }

        [Required]
        [JsonPropertyName("lesson_id")]
        public long LessonId { get; set; }


    }
}
