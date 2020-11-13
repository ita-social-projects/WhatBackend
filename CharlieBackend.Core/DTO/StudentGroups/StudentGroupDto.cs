using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CharlieBackend.Core.DTO.StudentGroups
{
    public class StudentGroupDto
    {
        public long Id { get; set; }

        [Required]
        public long? CourseId { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime FinishDate { get; set; }

        public IList<long> StudentIds { get; set; }

        public IList<long> MentorIds { get; set; }

    }
}
