using CharlieBackend.Core.Entities;
using System;

namespace CharlieBackend.Panel.Models.Calendar
{
    public class CalendarEventOccurrenceViewModel
    {
        public long? Id { get; set; }

        public long StudentGroupId { get; set; }

        public DateTime EventStart { get; set; }

        public DateTime EventFinish { get; set; }

        public PatternType Pattern { get; set; }

        public int Color { get; set; }
    }
}
