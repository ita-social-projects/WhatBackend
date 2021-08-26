using System;
using System.Text;
using System.Collections.Generic;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class AverageStudentGroupVisitDto
    {
        public string Course { get; set; }

        public string StudentGroup { get; set; }

        public long AverageVisitPercentage { get; set; }
    }
}
