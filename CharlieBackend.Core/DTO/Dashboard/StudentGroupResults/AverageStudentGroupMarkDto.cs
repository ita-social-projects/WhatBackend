using System;
using System.Text;
using System.Collections.Generic;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class AverageStudentGroupMarkDto
    {
        public long CourseId { get; set; }

        public long StudentGroupId { get; set; }

        public double? AverageMark { get; set; }
    }
}
