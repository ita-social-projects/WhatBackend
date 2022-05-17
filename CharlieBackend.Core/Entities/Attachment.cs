using System;
using System.Collections.Generic;

namespace CharlieBackend.Core.Entities
{
    [Serializable]
    public partial class Attachment : BaseEntity
    {
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public long CreatedByAccountId { get; set; }

        public string ContainerName { get; set; }

        public string FileName { get; set; }

        public virtual Account Account { get; set; }

        public virtual ICollection<AttachmentOfHomework> AttachmentsOfHomework { get; set; }

        public virtual ICollection<AttachmentOfHomeworkStudent> AttachmentOfHomeworkStudents { get; set; }

        public virtual ICollection<AttachmentOfHomeworkStudentHistory> AttachmentOfHomeworkStudentsHistory { get; set; }
    }
}
