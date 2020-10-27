using System.Collections.Generic;
using System.Text.Json.Serialization;
using CharlieBackend.Core.Models.Visit;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.Lesson
{
    public class UpdateLessonModel : LessonModel
    {
        [Required]
        [JsonIgnore]
        public override long Id { get; set; }

        [Required]
        [JsonPropertyName("lesson_visits")]
        public virtual List<VisitModel> LessonVisits { get; set; }
    }
}
