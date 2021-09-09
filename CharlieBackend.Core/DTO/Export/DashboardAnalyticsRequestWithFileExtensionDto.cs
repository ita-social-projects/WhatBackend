using CharlieBackend.Business.Services.FileServices.ExportFileServices;
using CharlieBackend.Core.DTO.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.DTO.Export
{
    public class DashboardAnalyticsRequestWithFileExtensionDto<T> where T : Enum
    {
        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public T[] IncludeAnalytics { get; set; }

        public ExportFileExtension Extension { get; set; }

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
