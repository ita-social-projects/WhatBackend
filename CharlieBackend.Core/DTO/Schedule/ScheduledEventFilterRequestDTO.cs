using System;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Schedule
{
    public class ScheduledEventFilterRequestDTO
    {
        public long? CourseID;

        public long? MentorID;

        public long? GroupID;

        public long? ThemeID;

        public long? StudentAccountID;

        public long? EventOccurrenceID;

        [DataType(DataType.DateTime)]
        public DateTime? StartDate;

        [DataType(DataType.DateTime)]
        public DateTime? FinishDate;
    }
}
