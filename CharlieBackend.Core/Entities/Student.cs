using System;
using System.Collections.Generic;

namespace CharlieBackend.Core.Entities
{
    public partial class Student : BaseEntity
    {
        public Student()
        {
            StudentsOfGroups = new HashSet<StudentsOfGroups>();
            Visits = new HashSet<Visit>();
        }

        public long? IdAccount { get; set; }

        public virtual Account IdAccountNavigation { get; set; }
        public virtual ICollection<StudentsOfGroups> StudentsOfGroups { get; set; }
        public virtual ICollection<Visit> Visits { get; set; }
    }
}
