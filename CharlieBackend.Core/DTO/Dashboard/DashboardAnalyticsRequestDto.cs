using System;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class DashboardAnalyticsRequestDto<T> where T : Enum
    {
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime FinishDate { get; set; }

        [Required]
        public T[] IncludeAnalytics { get; set; }
    }
}
