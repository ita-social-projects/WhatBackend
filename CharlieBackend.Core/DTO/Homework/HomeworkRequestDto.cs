using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CharlieBackend.Core.DTO.Homework
{
    public class HomeworkRequestDto
    {
        public DateTime? DueDate { get; set; }

        public string TaskText { get; set; }

        [Required]
        public long StudentGroupId { get; set; }

        [Required]
        public long MentorId { get; set; }

        public virtual IList<long> AttachmentIds { get; set; }
    }
}
