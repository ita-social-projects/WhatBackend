using System;

namespace CharlieBackend.Panel.Models.Calendar
{
    public class CalendarScheduledEventModel
    {
        public string Theme { get; set; }

        public string MentorFirstName { get; set; }

        public string MentorLastName { get; set; }

        public string StudentGroup { get; set; }

        public DateTime EventStart { get; set; }

        public DateTime EventFinish { get; set; }
    }
}
