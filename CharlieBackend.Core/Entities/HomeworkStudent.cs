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

        public long? MarkId { get; set; }

        public DateTime PublishingDate { get; set; }

        public bool IsSent { get; set; }

        public Mark Mark { get; set; }

        public Student Student { get; set; }

        public Homework Homework { get; set; }

        public ICollection<AttachmentOfHomeworkStudent> AttachmentOfHomeworkStudents { get; set; }
    }
}
