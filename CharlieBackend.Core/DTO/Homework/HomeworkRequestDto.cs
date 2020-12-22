using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CharlieBackend.Core.DTO.Homework
{
    public class HomeworkRequestDto
    {
        public ushort? DeadlineDays { get; set; }

        public string TaskText { get; set; }

        public bool IsCommon { get; set; }

        [Required]
        public long ThemeId { get; set; }

        [Required]
        public long MentorId { get; set; }

        public virtual IList<long> AttachmentIds { get; set; }
    }
}
