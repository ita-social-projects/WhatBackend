using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class StudentsRequestDto<T> where T : Enum
    {
        public long? CourseId { get; set; }

        public long? StudentGroupId { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime FinishDate { get; set; }

        [Required]
        public T[] IncludeAnalytics { get; set; }
    }
}
