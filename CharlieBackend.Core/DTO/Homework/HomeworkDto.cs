using System;
using System.Text;
using System.Collections.Generic;

namespace CharlieBackend.Core.DTO.Homework
{
    public class HomeworkDto
    {
        public long Id { get; set; }

        public ushort? DeadlineDays { get; set; }

        public string TaskText { get; set; }

        public bool IsCommon { get; set; }

        public long? ThemeId { get; set; }

        public long? MentorId { get; set; }

        public virtual IList<long> AttachmentIds { get; set; }
    }
}
