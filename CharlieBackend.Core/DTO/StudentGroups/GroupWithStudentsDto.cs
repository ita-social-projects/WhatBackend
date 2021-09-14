using CharlieBackend.Core.DTO.Student;
using System;
using System.Collections.Generic;

namespace CharlieBackend.Core.DTO.StudentGroups
{
    public class GroupWithStudentsDto
    {
        public long Id { get; set; }

        public long? CourseId { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public IList<StudentDto> Students { get; set; }
    }
}
