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
        public IList<CourseViewModel> Courses { get; set; }

        public IList<MentorViewModel> Mentors { get; set; }

        public IList<StudentGroupViewModel> StudentGroups { get; set; }

        public IList<StudentViewModel> Students { get; set; }

        public IList<ThemeViewModel> Themes { get; set; }

        public IList<EventOccurenceViewModel> EventOccurences { get; set; }

        public IList<ScheduledEventDTO> ScheduledEvents { get; set; }

        public ScheduledEventFilterRequestDTO ScheduledEventFilter { get; set; }
    }
}
