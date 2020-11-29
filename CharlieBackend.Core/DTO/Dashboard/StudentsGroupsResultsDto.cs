using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class StudentGroupsResultsDto
    {
        public IEnumerable<AverageStudentGroupVisitDto> AverageStudentGroupsVisits { get; set; }

        public IEnumerable<AverageStudentGroupMarkDto> AverageStudentGroupsMarks { get; set; }
    }
}
