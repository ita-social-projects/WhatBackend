using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.Entities
{
    public partial class ScheduleOfStudentGroup : BaseEntity
    {
        public long? ScheduleId { get; set; }

        public long? StudentGroupId { get; set; }

        public virtual StudentGroup StudentGroup { get; set; }

        public virtual Schedule Student { get; set; }
    }
}
