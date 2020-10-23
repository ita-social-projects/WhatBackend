using System.Collections.Generic;
using System.Text.Json.Serialization;
using CharlieBackend.Core.Models.Visit;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.Lesson
{
    public class CreateLessonModel : LessonModel
    {
        [JsonIgnore]
        public override long Id { get; set; }

        [Required]
        [JsonPropertyName("theme_name")]
        public override string ThemeName { get; set; }

        [JsonPropertyName("mentor_id")]
        public override long MentorId { get; set; }

        [Required]
        [JsonPropertyName("group_id")]
        public virtual long StudentGroupId { get; set; }

        [Required]
        [JsonPropertyName("lesson_visits")]
        public virtual List<VisitModel> LessonVisits { get; set; }

        [Required]
        [JsonPropertyName("lesson_date")]
        [DataType(DataType.DateTime)]
        public override string LessonDate { get; set; }
    }
}
