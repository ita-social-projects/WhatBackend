using System;
using System.Text;
using System.Collections.Generic;
using CharlieBackend.Core.DTO.Visit;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Lesson
{
    public class UpdateLessonDto
    {
        #nullable enable

        public string? ThemeName { get; set; }

        public DateTime LessonDate { get; set; }

        public IList<VisitDto>? LessonVisits { get; set; }

        #nullable disable
    }
}
