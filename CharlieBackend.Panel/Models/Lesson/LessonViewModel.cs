using CharlieBackend.Panel.Models.Mentor;
using CharlieBackend.Panel.Models.StudentGroups;
using System;

namespace CharlieBackend.Panel.Models.Lesson
{
    public class LessonViewModel
    {
        public long Id { get; set; }

        public string ThemeName { get; set; }

        public long MentorId { get; set; }

        public MentorViewModel Mentor { get; set; }

        public long? StudentGroupId { get; set; }

        public StudentGroupViewModel StudentGroup { get; set; }

        public DateTime LessonDate { get; set; }
    }
}
