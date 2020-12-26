using System;
using System.Collections.Generic;

namespace CharlieBackend.Core.Entities
{
    public partial class Homework : BaseEntity
    {
        public DateTime? DueDate { get; set; }

        public string TaskText { get; set; }

        public long StudentGroupId { get; set; }

        public StudentGroup StudentGroup { get; set; }

        public long MentorId { get; set; }

        public Mentor Mentor { get; set; }

        public virtual IList<AttachmentOfHomework> AttachmentsOfHomework { get; set; }
    }
}
