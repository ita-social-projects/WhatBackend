using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class StudentVisitDto
    {
        public long CourceId { get; set; }

        public long StudentGroupId { get; set; }

        public long StudentId { get; set; }

        public long? LessonId { get; set; }

        public DateTime? LessonDate { get; set; }

        public bool? Presence { get; set; }
    }
}
