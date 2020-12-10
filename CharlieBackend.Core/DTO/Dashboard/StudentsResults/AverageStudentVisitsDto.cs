using System;
using System.Text;
using System.Collections.Generic;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class AverageStudentVisitsDto
    {
        public long? CourseId { get; set; }

        public long StudentGroupId { get; set; }

        public long StudentId { get; set; }

        public int StudentAverageVisitsPercentage { get; set; }
    }
}
