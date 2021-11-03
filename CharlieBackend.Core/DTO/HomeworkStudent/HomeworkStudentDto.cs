using System;
using System.Collections.Generic;

namespace CharlieBackend.Core.DTO.HomeworkStudent
{
    public class HomeworkStudentDto
    {
        public long Id { get; set; }

        public long StudentId { get; set; }

        public string StudentName { get; set; }

        public long HomeworkId { get; set; }

        public string HomeworkText { get; set; }

        public DateTime PublishingDate { get; set; }

        public bool IsSent { get; set; }

        public HomeworkStudentMarkDto Mark { get; set; }
       
        public virtual IList<long> AttachmentIds { get; set; }
    }
}
