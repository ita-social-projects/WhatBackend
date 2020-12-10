using System;
using System.Text;
using System.Collections.Generic;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class AverageStudentGroupVisitDto
    {
        public long? CourseId { get; set; }

        public long StudentGroupId { get; set; }

        public long AverageVisitPercentage { get; set; }
    }
}
