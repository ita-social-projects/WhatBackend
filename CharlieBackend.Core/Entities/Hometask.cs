using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.Entities
{
    public partial class Hometask
    {
        public long Id { get; set; }

        public ushort? DeadlineDays { get; set; }

        public string? TaskText { get; set; }

        public virtual IList<AttachmentOfHometask> AttachmentOfHometask { get; set; }

        public string? Comment { get; set; }

        public bool PublicTask { get; set; }

        ???public long ThemeId { get; set; }??? // List?

        public long MentorId { get; set; }
    }
}
