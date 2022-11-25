using CharlieBackend.Core.DTO.Schedule;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Panel.Models.Calendar
{
    public enum CalendarDisplayType
    {
        [Display(Name = "Working week")]
        WorkingWeek = 1,

        [Display(Name = "Full week")]
        FullWeek = 2
    }

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

        public CalendarDisplayType DisplayType { get; set; }
    }
}
