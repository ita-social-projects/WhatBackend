using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;


namespace CharlieBackend.Core.DTO.Lesson
{
    public class AssignMentorToLessonDto
    {
        [Required]
        public long MentorId { get; set; }

        [Required]
        public long LessonId { get; set; }

    }
}
