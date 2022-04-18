using CharlieBackend.Core.DTO.Mentor;
using CharlieBackend.Panel.Models.Course;
using CharlieBackend.Panel.Models.StudentGroups;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Panel.Models.Mentor
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

        public IList<long> CourseIds { get; set; }

        public IList<MentorStudyGroupsDto> MentorStudyGroups { get; set; }
    }
}
