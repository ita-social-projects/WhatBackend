using CharlieBackend.Core.Models.Visit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

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
