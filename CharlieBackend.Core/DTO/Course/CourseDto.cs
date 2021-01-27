using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.DTO.Course
{
    public class CourseDto
    {
        public  long Id { get; set; }

        public  string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
