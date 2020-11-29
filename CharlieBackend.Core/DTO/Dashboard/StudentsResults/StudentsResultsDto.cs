using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class StudentsResultsDto
    {
        public IEnumerable<AverageStudentVisitsDto> AverageStudentVisits { get; set; }

        public IEnumerable<AverageStudentMarkDto> AverageStudentsMarks { get; set; }
    }
}
