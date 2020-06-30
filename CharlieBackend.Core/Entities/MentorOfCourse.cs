using System;
using System.Collections.Generic;

namespace CharlieBackend.Core.Entities
{
    public partial class MentorOfCourse : BaseEntity
    {
        public long? IdCourse { get; set; }
        public long? IdMentor { get; set; }
        public string MentorComment { get; set; }

        public virtual Course IdCourseNavigation { get; set; }
        public virtual Mentor IdMentorNavigation { get; set; }
    }
}
