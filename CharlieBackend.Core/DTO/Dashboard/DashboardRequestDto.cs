using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class DashboardRequestDto
    {
        public long? GroupId { get; set; }

        public long? CourceId { get; set; }

        public DashboardResultType[] IncludeAnalytics { get; set; }
    }

    public enum DashboardResultType
    {
        AverageStudentMark,
        AverageStudentVisits,
        Classbook,
    }
}
