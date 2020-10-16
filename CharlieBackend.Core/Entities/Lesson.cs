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

        public long? MentorId { get; set; }

        public long? StudentGroupId { get; set; }

        public long? ThemeId { get; set; }

        public DateTime LessonDate { get; set; }

        public virtual Mentor Mentor { get; set; }

        public virtual StudentGroup StudentGroup { get; set; }

        public virtual Theme Theme { get; set; }
        
        public virtual ICollection<Visit> Visits { get; set; }
    }
}
