using System.Collections.Generic;

namespace CharlieBackend.Core.Models.StudentGroup
{
    public class StudentGroupModel
    {
        public string name { get; set; }
        public int course_id { get; set; }
        public string start_date { get; set; }
        public string finish_date { get; set; }
        public IEnumerable<int> students_id { get; set; }
    }
}
