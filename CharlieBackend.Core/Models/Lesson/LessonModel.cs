using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.Lesson
{
    public class LessonModel
    {
        [Required]
        public virtual long Id { get; set; }

        [Required]
        [JsonPropertyName("theme_name")]
        [StringLength(100)]
        public virtual string ThemeName { get; set; }
        [JsonPropertyName("mentor_id")]
        public virtual long MentorId { get; set; }

        [JsonPropertyName("lesson_date")]
        [DataType(DataType.DateTime)]
        public virtual DateTime LessonDate { get; set; }

        //public virtual long GroupId { get; set; }
        //public virtual VisitModel[] LessonVisits { get; set; }
    }
}
