using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.Course
{
    public class CourseModel
    {
        [Required]
        public long Id { get; set; }
       
       // [Required]//can be null in bd
        [StringLength(100)]
        public string Name { get; set; }

    }
}
