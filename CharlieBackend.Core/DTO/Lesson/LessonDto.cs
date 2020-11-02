using System;
using System.Text;
using System.Collections.Generic;
using CharlieBackend.Core.DTO.Visit;

namespace CharlieBackend.Core.DTO.Lesson
{
    public class LessonDto
    {
        public long Id { get; set; }

        public string ThemeName { get; set; }

        public long MentorId { get; set; }

        public long? StudentGroupId { get; set; }

        public DateTime LessonDate { get; set; }

        public virtual IList<VisitDto> Visits { get; set; }
    }
}
