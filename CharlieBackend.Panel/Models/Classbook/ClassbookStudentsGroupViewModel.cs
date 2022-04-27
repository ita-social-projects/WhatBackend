using System;
using System.Collections.Generic;

namespace CharlieBackend.Panel.Models.Classbook
{
    public class ClassbookStudentsGroupViewModel
    {
        public long Id { get; set; }

        public long? CourseId { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public IList<long> StudentIds { get; set; }

        public IList<long> MentorIds { get; set; }
    }
}
