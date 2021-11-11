using System;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class DashboardAnalyticsRequestDto<T> where T : Enum
    {
        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public T[] IncludeAnalytics { get; set; }
    }
}
