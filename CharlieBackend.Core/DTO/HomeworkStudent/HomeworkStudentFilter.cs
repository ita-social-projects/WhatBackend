using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.DTO.HomeworkStudent
{
    public class HomeworkStudentFilter
    {
        public long GroupId {get; set;}
        public DateTime? StartDate { get; set; }
        public DateTime? FinishtDate { get; set; }

    }
}
