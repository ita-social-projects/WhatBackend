using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.Entities
{
    public partial class Mark : BaseEntity
    {
        public int Value { get; set; }

        public string Comment { get; set; }

        public DateTime EvaluationDate { get; set; }

        public MarkType Type { get; set; }

        public virtual HomeworkStudent HomeworkStudent { get; set; }
    }
}
