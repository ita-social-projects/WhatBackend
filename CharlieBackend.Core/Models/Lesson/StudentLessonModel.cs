using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.Lesson
{
    public class StudentLessonModel
    {
        [Required]
        [JsonPropertyName("theme_name")]
        [StringLength(100)]
        public virtual string ThemeName { get; set; }

        [Required]
        public virtual long Id { get; set; }

        [Required]
        public virtual bool Presence { get; set; }

        public virtual sbyte? Mark { get; set; }

        [StringLength(1024)]
        public virtual string Comment { get; set; }

        [JsonPropertyName("student_group_id")]
        public virtual long? StudentGroupId { get; set; }

        [JsonPropertyName("lesson_date")]
        [DataType(DataType.DateTime)]
        public virtual DateTime LessonDate { get; set; }
    }
}
