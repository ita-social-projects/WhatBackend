
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.DTO.StudentGroups
{
    public class CreateStudentGroupDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public long CourseId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FinishDate { get; set; }

        [Required]
        public IList<long> StudentIds { get; set; }

        [Required]
        public IList<long> MentorIds { get; set; }
    }
}
