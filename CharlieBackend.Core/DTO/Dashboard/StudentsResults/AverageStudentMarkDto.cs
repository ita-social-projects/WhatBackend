using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class AverageStudentMarkDto
    {
        public long CourceId { get; set; }

        public long StudentGroupId { get; set; }

        public long StudentId { get; set; }

        public double? StudentAverageMark { get; set; }
    }
}
