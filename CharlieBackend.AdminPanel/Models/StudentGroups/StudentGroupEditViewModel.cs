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
    public class StudentGroupEditViewModel
    {
        public long Id { get; set; }

        public IList<CourseViewModel> AllCourses { get; set; }

        public CourseViewModel ActiveCourse { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime FinishDate { get; set; }

        public IList<StudentViewModel> AllStudents { get; set; }

        public IList<StudentViewModel> ActiveStudents { get; set; }

        public IList<MentorViewModel> AllMentors { get; set; }

        public IList<MentorViewModel> ActiveMentors { get; set; }
    }
}
