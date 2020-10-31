using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.Entities
{
    public partial class Schedule : BaseEntity
    {
        public DateTime LessonStart { get; set; }
        public DateTime LessonEnd { get; set; }
        public virtual StudentGroup StudentGroup { get; set; }
    }
}
