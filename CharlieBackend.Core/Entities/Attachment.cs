using System;

namespace CharlieBackend.Core.Entities
{
    public partial class Attachment : BaseEntity
    {
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public long CreatedByAccountId { get; set; }

        public string ContainerName { get; set; }

        public string FileName { get; set; }
    }
}
