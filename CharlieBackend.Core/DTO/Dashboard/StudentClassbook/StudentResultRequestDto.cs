using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CharlieBackend.Core.DTO.Dashboard.StudentClassbook
{
    public class StudentResultRequestDto
    {
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime FinishtDate { get; set; }

        public StudentResultType[] IncludeAnalytics { get; set; }
    }
}
