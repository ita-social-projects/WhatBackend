using System;
using System.Text;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class StudentResultsOfStudentGroupDto
    {
        public long? StudentId { get; set; }

        public double? StudentAverageMark { get; set; }

        public int? StudentVisitsPercentage { get; set; }

        public IList<LessonResultDto> LessonsResultDto { get; set; }
    }
}
