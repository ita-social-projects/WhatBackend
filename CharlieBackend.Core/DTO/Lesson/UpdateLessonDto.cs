using CharlieBackend.Core.DTO.Visit;
using System;
using System.Collections.Generic;

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
