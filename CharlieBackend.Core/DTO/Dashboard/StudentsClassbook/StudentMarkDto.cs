using System;
using System.Text;
using System.Collections.Generic;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class StudentMarkDto
    {
        public long? CourseId { get; set; }

        public long StudentGroupId { get; set; }

        public long StudentId { get; set; }

        public long? LessonId { get; set; }

        public DateTime? LessonDate { get; set; }

        public sbyte? StudentMark { get; set; }

        public string Comment { get; set; }
    }
}
