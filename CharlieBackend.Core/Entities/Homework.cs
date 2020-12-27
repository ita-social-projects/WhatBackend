﻿using System;
using System.Collections.Generic;

namespace CharlieBackend.Core.Entities
{
    public partial class Homework : BaseEntity
    {
        public ushort? DeadlineDays { get; set; }

        public string TaskText { get; set; }

        public bool IsCommon { get; set; }

        public long ThemeId { get; set; }

        public Theme Theme { get; set; }

        public long MentorId { get; set; }

        public Mentor Mentor { get; set; }

        public virtual IList<AttachmentOfHomework> AttachmentsOfHomework { get; set; }
    }
}
