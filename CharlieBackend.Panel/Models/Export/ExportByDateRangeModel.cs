using System;

namespace CharlieBackend.Panel.Models.Export
{
    public class ExportByDateRangeModel
    {
        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public int[] IncludeAnalytics { get; set; }
    }
}
