using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace CharlieBackend.Core.DTO.StudentGroups
{
    public class UpdateStudentsForStudentGroup
    {
        [Required]
        public IList<long> StudentIds { get; set; }

    }
}
