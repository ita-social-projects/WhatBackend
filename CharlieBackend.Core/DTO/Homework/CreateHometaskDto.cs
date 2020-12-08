using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CharlieBackend.Core.DTO.Homework
{
    public class CreateHometaskDto
    {
        public ushort? DeadlineDays { get; set; }

        public string? TaskText { get; set; }

        public virtual IList<long> AttachmentId { get; set; }

        public string? Comment { get; set; }

        [Required]
        public bool PublicTask { get; set; }

        [Required]
        ???public long ThemeId { get; set; }??? // List<>?

        [Required]
        public long MentorId { get; set; }
    }
}
