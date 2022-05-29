using System;

namespace CharlieBackend.Core.Entities
{
    public partial class Mark : BaseEntity
    {
        /// <summary>
        /// This property contains the value of the mark
        /// </summary>
        public sbyte Value { get; set; }

        /// <summary>
        /// This property contains the comment for the mark
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// This property contains the date and time when the mark was given
        /// </summary>
        public DateTime EvaluationDate { get; set; }

        /// <summary>
        /// This property contains the type of the mark
        /// </summary>
        /// <example> "0" - mark for homework, "1" - mark for visit </example>
        public MarkType Type { get; set; }

        /// <summary>
        /// This property contains the id of the account which created the mark
        /// </summary>
        public long EvaluatedBy { get; set; }

        public virtual Account Account { get; set; }

        public virtual HomeworkStudent HomeworkStudent { get; set; }

        public virtual HomeworkStudentHistory HomeworkStudentHistory { get; set; }
    }
}
