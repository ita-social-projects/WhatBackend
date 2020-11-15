using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class DashboardResultDto
    {
        public IEnumerable<AverageStudentVisitsDto> AverageStudentVisits { get; set; }

        public IEnumerable<AverageStudentMarkDto> AverageStudentsMarks { get; set; }

        public IEnumerable<StudentMarkDto> StudentsMarks { get; set; }

        public IEnumerable<StudentVisitDto> StudentsPresences { get; set; }
    }
}
