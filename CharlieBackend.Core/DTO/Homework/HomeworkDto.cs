using System;
using System.Text;
using System.Collections.Generic;

namespace CharlieBackend.Core.DTO.Homework
{
    public class HomeworkDto
    {
        public long Id { get; set; }

        public DateTime? DueDate { get; set; }

        public string TaskText { get; set; }

        public long LessonId { get; set; }

        public DateTime PublishingDate { get; set; }

        public long CreatedBy { get; set; }

        public virtual IList<long> AttachmentIds { get; set; }
        public string ThemeName { get; set; }
    }
}
