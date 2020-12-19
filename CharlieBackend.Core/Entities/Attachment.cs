using System;

namespace CharlieBackend.Core.Entities
{
    public partial class Attachment : BaseEntity
    {
        public DateTime AddTime { get; set; } = DateTime.Now;

        public long UserId { get; set; }

        public UserRole UserRole { get; set; }

        public string ContainerName { get; set; }

        public string FileName { get; set; }
    }
}
