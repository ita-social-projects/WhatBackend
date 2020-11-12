using System;
using System.Text;
using System.Collections.Generic;
using CharlieBackend.Core.DTO.Visit;
using System.Text.Json.Serialization;
using CharlieBackend.Core.Models.Visit;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Lesson
{
    public class CreateLessonDto
    {

        [Required]
        [StringLength(100)]
        public string ThemeName { get; set; }

        public long MentorId { get; set; }

        [Required]
        public long StudentGroupId { get; set; }

        [Required]
        public IList<VisitDto> LessonVisits { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime LessonDate { get; set; }
    }
}
