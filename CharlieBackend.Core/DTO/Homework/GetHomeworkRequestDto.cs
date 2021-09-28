using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.DTO.Homework
{
    public class GetHomeworkRequestDto
    {
        public long? GroupId { get; set; }
        public long? CourseId { get; set; }
        public long? ThemeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
    }
}