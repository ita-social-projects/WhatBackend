using System;
using System.Collections.Generic;

namespace CharlieBackend.Core.Entities
{
    public partial class Lesson : BaseEntity
    {
        public Lesson()
        {
            Visits = new HashSet<Visit>();
        }

        public long? IdMentor { get; set; }
        public long? IdStudentGroup { get; set; }
        public long? IdTheme { get; set; }
        public DateTime? LessonDate { get; set; }

        public virtual Mentor IdMentorNavigation { get; set; }
        public virtual StudentGroup IdStudentGroupNavigation { get; set; }
        public virtual Theme IdThemeNavigation { get; set; }
        public virtual ICollection<Visit> Visits { get; set; }
    }
}
