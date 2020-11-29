using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class AverageStudentGroupMarkDto
    {
        public long CourceId { get; set; }

        public long StudentGroupId { get; set; }

        public double? AverageMark { get; set; }
    }
}
