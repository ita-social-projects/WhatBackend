using System;

namespace CharlieBackend.Panel.Models.Lesson
{
    public class LessonViewModel
    {
        public long Id { get; set; }

        public string ThemeName { get; set; }

        public long MentorId { get; set; }

        public long? StudentGroupId { get; set; }

        public DateTime LessonDate { get; set; }

    }
}
