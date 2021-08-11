using System;
using System.Text;
using System.Collections.Generic;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class AverageStudentMarkDto
    {
        public string Course { get; set; }

        public string StudentGroup { get; set; }

        public string Student { get; set; }

        public decimal? StudentAverageMark { get; set; }
    }
}
