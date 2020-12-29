using System;
using System.Collections.Generic;

namespace CharlieBackend.Core.Entities
{
    public partial class StudentGroup : BaseEntity
    {
        public long CourseId { get; set; }

        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? FinishDate { get; set; }

        public virtual Course Course { get; set; }

        public virtual IList<Lesson> Lesson { get; set; }

        public virtual IList<MentorOfStudentGroup> MentorsOfStudentGroups { get; set; }
        
        public virtual IList<Schedule> Schedule { get; set; }
        
        public virtual IList<StudentOfStudentGroup> StudentsOfStudentGroups { get; set; }

        public virtual IList<Homework> Homeworks { get; set; }
    }
}
