
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.DTO.StudentGroups
{
    public class CreateStudentGroupDto
    {
        public string Name { get; set; }

        public long CourseId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public IList<long> StudentIds { get; set; }

        public IList<long> MentorIds { get; set; }
    }
}
