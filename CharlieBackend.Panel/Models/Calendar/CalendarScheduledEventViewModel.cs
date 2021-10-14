using System;

namespace CharlieBackend.Panel.Models.Calendar
{
    public class CalendarScheduledEventViewModel
    {
        public long Id { get; set; }

        public long EventOccuranceId { get; set; }

        public long StudentGroupId { get; set; }

        public long? ThemeId { get; set; }

        public long? MentorId { get; set; }

        public long? LessonId { get; set; }
        public string Name { get; set; } = "Test";

        public DateTime EventStart { get; set; }

        public DateTime EventFinish { get; set; }
    }
}
