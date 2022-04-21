using CharlieBackend.Panel.Models.Mentor;
using CharlieBackend.Panel.Models.StudentGroups;
using CharlieBackend.Panel.Models.Theme;
using System;
using System.Collections.Generic;

namespace CharlieBackend.Panel.Models.ScheduledEvent
{
    public class ScheduledEventEditViewModel
    {
        public long Id { get; set; }

        public long EventOccuranceId { get; set; }

        public long StudentGroupId { get; set; }

        public long? ThemeId { get; set; }

        public long? MentorId { get; set; }

        public long? LessonId { get; set; }

        public DateTime EventStart { get; set; }

        public DateTime EventFinish { get; set; }

        public IList<MentorViewModel> AllMentors { get; set; }

        public IList<StudentGroupViewModel> AllStudentGroups { get; set; }

        public IList<ThemeViewModel> AllThemes { get; set; }
    }
}
