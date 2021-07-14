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

        public DateTime? StartDate { get; set; }

        public DateTime? FinishDate { get; set; }
    }
}
