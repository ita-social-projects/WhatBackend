using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.Course
{
    public class CourseModel
    {
        public virtual long Id { get; set; }

        [Required]
        public virtual string Name { get; set; }
    }
}
