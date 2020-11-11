using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.FileModels
{
    public class StudentGroupFileModel
    {
        public long? Id { get; set; }

        public long CourseId { get; set; }

        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? FinishDate { get; set; }
    }
}
