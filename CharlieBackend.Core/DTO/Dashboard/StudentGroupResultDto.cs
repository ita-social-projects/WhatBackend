using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class DashboardDto
    {
        public IEnumerable<WhateverDto> AverageVisits { get; set; }
        public IEnumerable<WhateverMarkDto> AverageMarks { get; set; }
        public IEnumerable<WhateverMarkDto> LessonResults { get; set; }
    }

    public class StudentGroupResultDto
    {
        public long StudentGroupId { get; set; }

        public long CourceId { get; set; }

        public IList<StudentResultsOfStudentGroupDto> StudentsResultsOfStudentGroup { get; set; }
    }
}
