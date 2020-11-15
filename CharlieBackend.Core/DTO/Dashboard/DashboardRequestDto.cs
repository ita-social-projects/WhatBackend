using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class DashboardRequestDto
    {
        public long? GroupId { get; set; }

        public long? CourseId { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        public DashboardResultType[] IncludeAnalytics { get; set; }
    }

    public enum DashboardResultType
    {
        AverageStudentMark,
        AverageStudentVisits,
        StudentPresence,
        StudentMarks,
    }
}
