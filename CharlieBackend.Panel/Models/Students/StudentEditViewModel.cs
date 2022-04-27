using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Panel.Models.StudentGroups;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Panel.Models.Students
{
    public class StudentEditViewModel
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

        public IList<StudentStudyGroupsDto> StudentStudyGroups { get; set; }
    }
}
