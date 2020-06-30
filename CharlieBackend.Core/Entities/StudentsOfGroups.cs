using System;
using System.Collections.Generic;

namespace CharlieBackend.Core.Entities
{
    public partial class StudentsOfGroups : BaseEntity
    {
        public long IdStudentGroup { get; set; }
        public long IdStudent { get; set; }

        public virtual StudentGroup IdStudentGroupNavigation { get; set; }
        public virtual Student IdStudentNavigation { get; set; }
    }
}
