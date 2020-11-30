using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class StudentGroupsResultsRequestDto
    {
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime FinishDate { get; set; }

        [Required]
        public StudentGroupResultType[] IncludeAnalytics { get; set; }
    }
}
