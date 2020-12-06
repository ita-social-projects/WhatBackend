using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CharlieBackend.Core.DTO.Mentor
{
    class MentorStudyGroupsDto
    {
        public long Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }
    }
}
