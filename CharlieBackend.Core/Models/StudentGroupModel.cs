using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.Models
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
