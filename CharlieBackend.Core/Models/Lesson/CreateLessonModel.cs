using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.Lesson
{
    public class CreateLessonModel : LessonModel
    {
        [JsonIgnore]
        public override long Id { get; set; }

        [Required]
        public override string ThemeName { get; set; }

        [Required]
        public override string LessonDate { get; set; }
    }
}
