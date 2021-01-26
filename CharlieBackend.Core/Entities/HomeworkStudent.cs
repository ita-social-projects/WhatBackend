using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.Entities
{
    public partial class HomeworkStudent : BaseEntity
    {
        public long StudentId { get; set; }

        public long HomeworkId { get; set; }

        public string HomeworkText { get; set; }

        public Homework Homework { get; set; }

        public ICollection<AttachmentOfHomeworkStudent> AttachmentOfHomeworkStudents { get; set; }
    }
}
