using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;


namespace CharlieBackend.Core.DTO.Lesson
{
    public class AssignMentorToLessonDto
    {
        public long MentorId { get; set; }

        public long LessonId { get; set; }

    }
}
