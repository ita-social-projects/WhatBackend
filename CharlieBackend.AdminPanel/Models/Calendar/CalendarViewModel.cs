using CharlieBackend.AdminPanel.Models.Course;
using CharlieBackend.AdminPanel.Models.EventOccurence;
using CharlieBackend.AdminPanel.Models.Mentor;
using CharlieBackend.AdminPanel.Models.StudentGroups;
using CharlieBackend.AdminPanel.Models.Students;
using CharlieBackend.AdminPanel.Models.Theme;
using CharlieBackend.Core.DTO.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
