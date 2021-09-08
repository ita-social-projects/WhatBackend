using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.Entities
{
    public partial class AttachmentOfHomeworkStudentHistory : BaseEntity
    {
        public long HomeworkStudentHistoryId { get; set; }

        public long AttachmentId { get; set; }

        public HomeworkStudentHistory HomeworkStudentHistory { get; set; }

        public Attachment Attachment { get; set; }
    }
}
