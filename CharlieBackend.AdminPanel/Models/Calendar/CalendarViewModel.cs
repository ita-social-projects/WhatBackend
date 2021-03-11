using CharlieBackend.Core.DTO.Schedule;
using System.Collections.Generic;

namespace CharlieBackend.AdminPanel.Models.Calendar
{
    public class CalendarViewModel
    {
        public IList<CalendarCourseViewModel> Courses { get; set; }

        public IList<CalendarMentorViewModel> Mentors { get; set; }

        public IList<CalendarStudentGroupViewModel> StudentGroups { get; set; }

        public IList<CalendarStudentViewModel> Students { get; set; }

        public IList<CalendarThemeViewModel> Themes { get; set; }

        public IList<CalendarEventOccurrenceViewModel> EventOccurences { get; set; }

        public IList<CalendarScheduledEventViewModel> ScheduledEvents { get; set; }

        public ScheduledEventFilterRequestDTO ScheduledEventFilter { get; set; }
    }
}
