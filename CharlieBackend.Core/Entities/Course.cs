using System;
using System.Collections.Generic;

namespace CharlieBackend.Core.Entities
{
    public partial class Course : BaseEntity
    {
        public Course()
        {
            MentorsOfCourses = new HashSet<MentorOfCourse>();
            StudentGroups = new HashSet<StudentGroup>();
        }

        public string Name { get; set; }

        public virtual ICollection<MentorOfCourse> MentorsOfCourses { get; set; }
        public virtual ICollection<StudentGroup> StudentGroups { get; set; }
    }
}
