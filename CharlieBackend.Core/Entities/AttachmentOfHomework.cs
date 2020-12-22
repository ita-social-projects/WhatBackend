using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.Entities
{
    public partial class AttachmentOfHomework : BaseEntity
    {
        public long HomeworkId { get; set; }

        public long AttachmentId { get; set; }

        public virtual Attachment Attachment { get; set; }

        public virtual Homework Homework { get; set; }
    }
}
