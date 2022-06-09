using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Entities
{
    public partial class EventOccurrence : BaseEntity
    {
        [Required]
        public long StudentGroupId { get; set; }

        [Required]
        public virtual StudentGroup StudentGroup { get; set; }

        [Required]
        public DateTime EventStart { get; set; }

        [Required]
        public DateTime EventFinish { get; set; }

        [Required]
        [EnumDataType(typeof(PatternType))]
        public virtual PatternType Pattern { get; set; }

        [Required]
        public long Storage { get; set; }

        public int Color { get; set; }

        public virtual ICollection<ScheduledEvent> ScheduledEvents { get; set; }
    }

    public enum PatternType
    {
        Daily,
        Weekly,
        AbsoluteMonthly,
        RelativeMonthly
    }
}
