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

        /// <summary>
        /// This property contains the date and time when student added his homework
        /// </summary>
        public DateTime PublishingDate { get; set; }

        /// <summary>
        /// This property contains "true" if student decides to send his homework for checking. 
        /// This property contains "false" if student decides to send his homework later 
        /// (for instance: student hasn't finished his homework yet). 
        /// </summary>
        public bool IsSent { get; set; }

        public Mark Mark { get; set; }

        public Student Student { get; set; }

        public Homework Homework { get; set; }

        public ICollection<AttachmentOfHomeworkStudent> AttachmentOfHomeworkStudents { get; set; }
    }
}
