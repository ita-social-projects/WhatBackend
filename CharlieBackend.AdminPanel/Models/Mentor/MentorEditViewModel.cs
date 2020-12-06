﻿using CharlieBackend.AdminPanel.Models.Course;
using CharlieBackend.AdminPanel.Models.StudentGroups;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Models.Mentor
{
    public class MentorEditViewModel
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        public string LastName { get; set; }

        public IList<StudentGroupViewModel> AllGroups { get; set; }

        public IList<CourseViewModel> AllCourses { get; set; }
    }
}
