using System;

namespace CharlieBackend.Panel.Models.GroupsRegister
{
    public class FilteredRegisterViewModel
    {
        public long Id { get; set; }

        public long EventOccuranceId { get; set; }

        public long StudentGroupId { get; set; }

        public long? ThemeId { get; set; }

        public long? MentorId { get; set; }

        public long? LessonId { get; set; }

        public DateTime EventStart { get; set; }

        public DateTime EventFinish { get; set; }
    }
}
