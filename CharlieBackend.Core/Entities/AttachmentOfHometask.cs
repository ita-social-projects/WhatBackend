using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.Entities
{
    public class AttachmentOfHometask
    {
        public long Id { get; set; }

        public long? HometaskId { get; set; }

        public long? AttachmentId { get; set; }

        public virtual Attachment Attachment { get; set; }

        public virtual Hometask Hometask { get; set; }
    }
}
