using System.Collections.Generic;

namespace CharlieBackend.Core.Models.StudentGroup
{
    public class StudentGroupModel
    {
        public virtual long id { get; set; }
        public virtual string name { get; set; }
        public virtual long course_id { get; set; }
        public virtual string start_date { get; set; }
        public virtual string finish_date { get; set; }
        public virtual List<long> students_id { get; set; }
    }
}
