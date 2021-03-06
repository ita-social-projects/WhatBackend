using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.DTO.HomeworkStudent
{
    public class HomeworkStudentDto
    {
        public long Id { get; set; }

        public long StudentId { get; set; }

        public string StudentName { get; set; }

        public long HomeworkId { get; set; }

        public string HomeworkText { get; set; }

        public virtual IList<long> AttachmentIds { get; set; }
    }
}
