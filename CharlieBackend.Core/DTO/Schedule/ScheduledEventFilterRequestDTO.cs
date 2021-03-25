using System;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Schedule
{
    public class ScheduledEventFilterRequestDTO
    {
        public long? CourseID { get; set; }

        public long? MentorID { get; set; }

        public long? GroupID { get; set; }

        public long? ThemeID { get; set; }

        public long? StudentAccountID { get; set; }

        public long? EventOccurrenceID { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? FinishDate { get; set; }
    }
}
