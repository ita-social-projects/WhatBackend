using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CharlieBackend.Core.DTO.HomeworkStudent
{
    public class HomeworkStudentRequestDto
    {
        public long HomeworkId { get; set; }
       
        public string HomeworkText { get; set; }

        public bool IsSent { get; set; }

        public virtual IList<long> AttachmentIds { get; set; }
    }
}
