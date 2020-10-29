using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.Entities
{
    class Schedule : BaseEntity
    {
        public DateTime LessonStart { get; set; }
        public DateTime LessonEnd { get; set; }
        public virtual ICollection<StudentGroup> StudentGroups { get; set; }
        public virtual ICollection<Mentor> Mentors { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
