using CharlieBackend.Core.DTO.Visit;
using System;
using System.Collections.Generic;

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
