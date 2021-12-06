using System.Collections.Generic;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class StudentsResultsDto
    {
        public IEnumerable<AverageStudentVisitsDto> AverageStudentVisits { get; set; }

        public IEnumerable<AverageStudentMarkDto> AverageStudentsMarks { get; set; }
    }
}
