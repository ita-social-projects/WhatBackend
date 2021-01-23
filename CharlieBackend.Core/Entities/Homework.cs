using System;
using System.Collections.Generic;

namespace CharlieBackend.Core.Entities
{
    public partial class Homework : BaseEntity
    {
        public DateTime? DueDate { get; set; }

        public string TaskText { get; set; }

        public long LessonId { get; set; }

        public Lesson Lesson { get; set; }

        public virtual IList<AttachmentOfHomework> AttachmentsOfHomework { get; set; }
    }
}
