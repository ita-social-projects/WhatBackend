using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.DTO.Homework
{
    public class HometaskDto
    {
        public long Id { get; set; }

        public ushort? DeadlineDays { get; set; }

        public string TaskText { get; set; }

        public virtual IList<long> AttachmentId { get; set; }

        public string Comment { get; set; }

        public bool Common { get; set; }

        public long? ThemeId { get; set; }

        public long? MentorId { get; set; }
    }
}
