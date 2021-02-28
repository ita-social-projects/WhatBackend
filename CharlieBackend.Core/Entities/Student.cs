using System.Collections.Generic;

namespace CharlieBackend.Core.Entities
{
    public partial class Student : BaseEntity
    {
        public Student()
        {
            StudentsOfStudentGroups = new HashSet<StudentOfStudentGroup>();
            Visits = new HashSet<Visit>();
        }
        
        public long? AccountId { get; set; }

        public virtual Account Account { get; set; }

        public virtual ICollection<StudentOfStudentGroup> StudentsOfStudentGroups { get; set; }
        
        public virtual ICollection<HomeworkStudent> HomeworkStudents { get; set; }

        public virtual ICollection<Visit> Visits { get; set; }
    }
}
