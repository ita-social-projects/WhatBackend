using System;
using System.Collections.Generic;

namespace CharlieBackend.Core.Entities
{
    public partial class MentorOfStudentGroup : BaseEntity
    {
        public long? IdMentor { get; set; }
        public long? IdStudentGroup { get; set; }
        public string Comments { get; set; }

        public virtual Mentor IdMentorNavigation { get; set; }
        public virtual StudentGroup IdStudentGroupNavigation { get; set; }
    }
}
