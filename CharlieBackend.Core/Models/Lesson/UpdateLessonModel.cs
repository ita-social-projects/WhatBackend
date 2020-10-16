using System.Collections.Generic;
using System.Text.Json.Serialization;
using CharlieBackend.Core.Models.Visit;

namespace CharlieBackend.Core.Models.Lesson
{
    public class UpdateLessonModel : LessonModel
    {
        [JsonIgnore]
        public override long Id { get; set; }

        [JsonPropertyName("lesson_visits")]
        public virtual List<VisitModel> LessonVisits { get; set; }
    }
}
