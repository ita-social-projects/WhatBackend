using System;
using System.Collections.Generic;

namespace CharlieBackend.Core.Entities
{
    public partial class MentorOfCourse : BaseEntity
    {
        public long? CourseId { get; set; }
        public long? MentorId { get; set; }
        public string MentorComment { get; set; }

        public virtual Course Course { get; set; }
        public virtual Mentor Mentor { get; set; }
    }
}
