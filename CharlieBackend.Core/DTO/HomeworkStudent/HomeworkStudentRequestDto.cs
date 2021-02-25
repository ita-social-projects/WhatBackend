using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CharlieBackend.Core.DTO.HomeworkStudent
{
    public class HomeworkStudentRequestDto
    {
        [Required]
        public long HomeworkId { get; set; }
       
        [Required]
        public string HomeworkText { get; set; }

        public virtual IList<long> AttachmentIds { get; set; }
    }
}
