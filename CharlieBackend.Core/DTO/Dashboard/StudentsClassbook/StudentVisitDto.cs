using System;
using System.Text;
using System.Collections.Generic;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class StudentVisitDto
    {
        public string Course { get; set; }

        public string StudentGroup { get; set; }

        public string Student { get; set; }

        public long? LessonId { get; set; }

        public DateTime? LessonDate { get; set; }

        public bool? Presence { get; set; }
    }
}
