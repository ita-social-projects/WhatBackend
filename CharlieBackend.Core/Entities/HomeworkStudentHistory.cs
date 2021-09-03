using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.Entities
{
    public partial class HomeworkStudentHistory : BaseEntity
    {
        public string HomeworkText { get; set; }

        public long HomeworkStudentId { get; set; }

        public long MarkId { get; set; }

        /// <summary>
        /// This property contains the date and time when student added his homework
        /// </summary>
        public DateTime PublishingDate { get; set; }

        public Mark Mark { get; set; }

        public HomeworkStudent HomeworkStudent { get; set; }

        public ICollection<AttachmentOfHomeworkStudentHistory> AttachmentOfHomeworkStudentsHistory { get; set; }
    }
}
