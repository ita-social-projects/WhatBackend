using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class AverageStudentGroupVisitDto
    {
        public long CourceId { get; set; }

        public long StudentGroupId { get; set; }

        public long AverageVisitPercentage { get; set; }
    }
}
