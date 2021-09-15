using CharlieBackend.Core.DTO.Dashboard;
using System;

namespace CharlieBackend.Core.DTO.Export
{
    public class DashboardAnalyticsRequestWithFileExtensionDto<T> where T : Enum
    {
        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public T[] IncludeAnalytics { get; set; }

        public FileExtension Extension { get; set; }

        public DashboardAnalyticsRequestDto<T> GetDashboardAnalyticsRequestDto()
        {
            return new DashboardAnalyticsRequestDto<T>
            {
                StartDate = StartDate,
                FinishDate = FinishDate,
                IncludeAnalytics = IncludeAnalytics
            };
        }
    }
}
