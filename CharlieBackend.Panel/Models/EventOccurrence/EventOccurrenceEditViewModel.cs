using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using CharlieBackend.Panel.Models.Mentor;
using CharlieBackend.Panel.Models.StudentGroups;
using CharlieBackend.Panel.Models.Theme;
using System;
using System.Collections.Generic;

namespace CharlieBackend.Panel.Models.EventOccurrence
{
    public class EventOccurrenceEditViewModel: EventOccurrenceViewModel
    {
        public IList<MentorViewModel> AllMentors { get; set; }

        public IList<StudentGroupViewModel> AllStudentGroups { get; set; }

        public IList<ThemeViewModel> AllThemes { get; set; }

        public IList<ScheduledEventDTO> Events { get; set; }

        public PatternType Pattern { get; set; }

        public DetailedEventOccurrenceDTO DetailedEventOccurrence { get; set; }
    }
}
