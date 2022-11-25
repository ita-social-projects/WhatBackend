using System.Collections.Generic;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class StudentsResultsDto
    {
        public IEnumerable<AverageStudentVisitsDto> AverageStudentVisits { get; set; } = new List<AverageStudentVisitsDto>();

        public IEnumerable<AverageStudentMarkDto> AverageStudentsMarks { get; set; } = new List<AverageStudentMarkDto>();

        public IEnumerable<AverageStudentMarkDto> AverageStudentHomeworkMarks { get; set; } = new List<AverageStudentMarkDto>();
    }
}
