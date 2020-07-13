using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.Lesson
{
    public class StudentLessonModel
    {
        [JsonPropertyName("theme_name")]
        public virtual string ThemeName { get; set; }
        public virtual long Id { get; set; }
        public virtual bool Presence { get; set; }
        public virtual sbyte? Mark { get; set; }
        public virtual string Comment { get; set; }

        [JsonPropertyName("student_group_id")]
        public virtual long? StudentGroupId { get; set; }

        [JsonPropertyName("lesson_date")]
        [DataType(DataType.DateTime)]
        public virtual string LessonDate { get; set; }
    }
}
