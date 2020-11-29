using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class AverageStudentVisitsDto
    {
        public long CourceId { get; set; }

        public long StudentGroupId { get; set; }

        public long StudentId { get; set; }

        public int StudentAverageVisitsPercentage { get; set; }
    }
}
