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

        public long LessonId { get; set; }

        public virtual IList<long> AttachmentIds { get; set; }
    }
}
