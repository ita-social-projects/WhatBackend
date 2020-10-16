using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.Lesson
{
    public class LessonModel
    {
        public virtual long Id { get; set; }

        [JsonPropertyName("theme_name")]
        public virtual string ThemeName { get; set; }

        [JsonPropertyName("lesson_date")]
        [DataType(DataType.DateTime)]
        public virtual string LessonDate { get; set; }

        //public virtual long GroupId { get; set; }
        //public virtual VisitModel[] LessonVisits { get; set; }
    }
}
