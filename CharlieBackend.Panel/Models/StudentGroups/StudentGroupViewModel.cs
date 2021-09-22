using CharlieBackend.Panel.Models.Course;
using CharlieBackend.Panel.Models.Mentor;
using CharlieBackend.Panel.Models.Students;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Models.StudentGroups
{
    public class StudentGroupViewModel
    {
        public long Id { get; set; }

        [Required]
        public CourseViewModel Course { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime FinishDate { get; set; }

        public IList<StudentViewModel> Students { get; set; }

        public IList<MentorViewModel> Mentors { get; set; }
    }
}
