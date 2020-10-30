using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.Entities
{
    public partial class ScheduleOfStudent : BaseEntity
    {
        public long? ScheduleId { get; set; }

        public long? StudentId { get; set; }

        public virtual Student Student { get; set; }
        
        public virtual Schedule Schedule { get; set; }
    }
}
