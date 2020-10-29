using System;
using System.Collections.Generic;

namespace CharlieBackend.Core.Entities
{
    public partial class StudentGroup : BaseEntity
    {
        public StudentGroup()
        {
            Lesson = new HashSet<Lesson>();
            MentorsOfStudentGroups = new HashSet<MentorOfStudentGroup>();
            StudentsOfStudentGroups = new HashSet<StudentOfStudentGroup>();
        }

        public long? CourseId { get; set; }

        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? FinishDate { get; set; }

        public virtual Course Course { get; set; }

        public virtual ICollection<Lesson> Lesson { get; set; }

        public virtual ICollection<MentorOfStudentGroup> MentorsOfStudentGroups { get; set; }
        
        public virtual ICollection<StudentOfStudentGroup> StudentsOfStudentGroups { get; set; }
        public virtual ICollection<Schedule> Schedule { get; set; }
    }
}
