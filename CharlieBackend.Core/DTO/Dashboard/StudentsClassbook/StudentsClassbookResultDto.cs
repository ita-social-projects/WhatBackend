using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class StudentsClassbookResultDto
    {
        public IEnumerable<StudentMarkDto> StudentsMarks { get; set; }

        public IEnumerable<StudentVisitDto> StudentsPresences { get; set; }
    }
}
