using System;
using System.Collections.Generic;

namespace CharlieBackend.Core.Entities
{
    public partial class Mentor : BaseEntity
    {
        public Mentor()
        {
            Lessons = new HashSet<Lesson>();
            MentorsOfCourses = new HashSet<MentorOfCourse>();
            MentorsOfStudentGroups = new HashSet<MentorOfStudentGroup>();
        }

        public long? IdAccount { get; set; }

        public virtual Account IdAccountNavigation { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<MentorOfCourse> MentorsOfCourses { get; set; }
        public virtual ICollection<MentorOfStudentGroup> MentorsOfStudentGroups { get; set; }
    }
}
