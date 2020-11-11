using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Mentor
{
    public class UpdateMentorDto
    { 
        [EmailAddress]
        [StringLength(50)]
        public string? Email { get; set; }

        [StringLength(30)]
        public string? FirstName { get; set; }

        [StringLength(30)]
        public string? LastName { get; set; }

        public IList<long>? CourseIds { get; set; }

        public IList<long>? StudentGroupIds { get; set; }
    }
}
