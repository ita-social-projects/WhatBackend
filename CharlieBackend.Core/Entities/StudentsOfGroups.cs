using System;
using System.Collections.Generic;

namespace CharlieBackend.Core.Entities
{
    public partial class StudentsOfGroups : BaseEntity
    {
        public long StudentGroupId { get; set; }
        public long StudentId { get; set; }

        public virtual StudentGroup StudentGroup{ get; set; }
        public virtual Student Student { get; set; }
    }
}
