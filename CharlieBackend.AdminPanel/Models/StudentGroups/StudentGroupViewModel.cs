using CharlieBackend.AdminPanel.Models.Course;
using CharlieBackend.AdminPanel.Models.Mentor;
using CharlieBackend.AdminPanel.Models.Students;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Models.StudentGroups
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
