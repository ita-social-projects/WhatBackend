using System;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.StudentGroups
{
    public class ImportStudentGroupDto
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
    }
}
