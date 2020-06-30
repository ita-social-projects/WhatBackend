using System;
using System.Collections.Generic;

namespace CharlieBackend.Core.Entities
{
    public partial class Visit : BaseEntity
    {
        public long? IdStudent { get; set; }
        public long? IdLesson { get; set; }
        public sbyte? StudentMark { get; set; }
        public bool Presence { get; set; }
        public string Comments { get; set; }

        public virtual Lesson IdLessonNavigation { get; set; }
        public virtual Student IdStudentNavigation { get; set; }
    }
}
