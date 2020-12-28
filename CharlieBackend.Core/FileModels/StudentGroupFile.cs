using System;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.FileModels
{
    public class StudentGroupFile
    {
        public string Id { get; set; }

        [Required]
        public string CourseId { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FinishDate { get; set; }
    }
}
