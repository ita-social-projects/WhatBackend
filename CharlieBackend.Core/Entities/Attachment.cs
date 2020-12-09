using System.Collections.Generic;

namespace CharlieBackend.Core.Entities
{
    public partial class Attachment : BaseEntity
    {
        public string ContainerName { get; set; }

        public string FileName { get; set; }

        public virtual ICollection<AttachmentOfHometask> AttachmentOfHometasks { get; set; }
    }
}
