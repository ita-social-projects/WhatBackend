using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;   

namespace CharlieBackend.Core.Entities
{
    public partial class Schedule : BaseEntity
    {
        public TimeSpan LessonStart { get; set; }

        public TimeSpan LessonEnd { get; set; }

        public long? StudentGroupId { get; set; }

        public virtual StudentGroup StudentGroup { get; set; }

        public virtual RepeatRate RepeatRate { get; set; }

        [Range(1, 31)]   
        public uint? DayNumber { get; set; }
    }

    public enum RepeatRate{
        Never,
        Daily,
        Weekly,
        Monthly
    }
}
