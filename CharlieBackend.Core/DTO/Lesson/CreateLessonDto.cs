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
        public string ThemeName { get; set; }

        public long MentorId { get; set; }

        public long StudentGroupId { get; set; }

        public IList<VisitDto> LessonVisits { get; set; }

        public DateTime LessonDate { get; set; }
    }
}
