using System;
using System.Collections.Generic;

namespace CharlieBackend.Core.Entities
{
    public partial class StudentGroup : BaseEntity
    {
        public StudentGroup()
        {
            Lessons = new HashSet<Lesson>();
            MentorsOfStudentGroups = new HashSet<MentorOfStudentGroup>();
            StudentsOfGroups = new HashSet<StudentsOfGroups>();
        }

        public long? CourseId { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }

        public virtual Course Course { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<MentorOfStudentGroup> MentorsOfStudentGroups { get; set; }
        public virtual ICollection<StudentsOfGroups> StudentsOfGroups { get; set; }
    }
}
