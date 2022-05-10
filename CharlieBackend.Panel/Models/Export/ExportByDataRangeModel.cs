using System;

namespace CharlieBackend.Panel.Models.Export
{
    public class ExportByDataRangeModel
    {
        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public int[] IncludeAnalytics { get; set; }
    }
}
