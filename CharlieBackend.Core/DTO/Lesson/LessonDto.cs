using System;
using System.Text;
using System.Collections.Generic;

namespace CharlieBackend.Core.DTO.Lesson
{
    public class LessonDto
    {
        public long Id { get; set; }

        public string ThemeName { get; set; }

        public long MentorId { get; set; }

        public DateTime LessonDate { get; set; }
    }
}
