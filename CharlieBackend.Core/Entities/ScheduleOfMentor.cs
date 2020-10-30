using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.Entities
{
    public partial class ScheduleOfMentor : BaseEntity
    {
        public long? ScheduleId { get; set; }

        public long? MentorId { get; set; }

        public virtual Mentor Mentor { get; set; }
        
        public virtual Schedule Schedule { get; set; }
    }
}
