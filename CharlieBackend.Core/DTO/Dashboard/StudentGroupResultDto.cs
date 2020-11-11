using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class StudentGroupResultDto
    {
        public long StudentGroupId { get; set; }

        public long CourceId { get; set; }

        public IList<StudentResultsOfStudentGroupDto> StudentsResultsOfStudentGroup { get; set; }
    }
}
