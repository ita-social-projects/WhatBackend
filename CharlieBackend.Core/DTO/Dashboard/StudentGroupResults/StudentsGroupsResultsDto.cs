using System.Collections.Generic;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class StudentGroupsResultsDto
    {
        public IEnumerable<AverageStudentGroupVisitDto> AverageStudentGroupsVisits { get; set; }

        public IEnumerable<AverageStudentGroupMarkDto> AverageStudentGroupsMarks { get; set; }
    }
}
