using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.Entities
{
    public partial class Hometask
    {
        public long Id { get; set; }

        public ushort? DeadlineDays { get; set; }

        public string TaskText { get; set; }

        public string Comment { get; set; }

        public bool Common { get; set; }

        public long? ThemeId { get; set; }

        public Theme Theme { get; set; }

        public long MentorId { get; set; }

        public Mentor Mentor { get; set; }

        public virtual ICollection<AttachmentOfHometask> AttachmentOfHometasks { get; set; }
    }
}
