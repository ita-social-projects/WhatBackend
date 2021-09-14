using System;
using System.Collections.Generic;

namespace CharlieBackend.Core.Entities
{
    public partial class Homework : BaseEntity
    {
        public DateTime? DueDate { get; set; }

        public string TaskText { get; set; }

        public long LessonId { get; set; }

        /// <summary>
        /// This property contains the date and time when the homework was published
        /// </summary>
        public DateTime PublishingDate { get; set; }

        /// <summary>
        /// This property contains the id of the account which created the homework
        /// </summary>
        public long CreatedBy { get; set; }

        public Lesson Lesson { get; set; }

        public virtual Account Account { get; set; }

        public virtual IList<AttachmentOfHomework> AttachmentsOfHomework { get; set; }

        public virtual IList<HomeworkStudent> HomeworkStudents { get; set; }
    }
}
