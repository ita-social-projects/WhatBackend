using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Models.Lesson
{
    public class StudentLessonViewModel
    {
        public string ThemeName { get; set; }

        public bool Presence { get; set; }

        public sbyte? Mark { get; set; }

        public string Comment { get; set; }

        public long? StudentGroupId { get; set; }

        public DateTime LessonDate { get; set; }
    }
}
