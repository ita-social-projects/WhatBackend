using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.DTO.StudentGroups
{
    public class UpdateStudentsForStudentGroup
    {
        [Required]
        [JsonPropertyName("student_ids")]
        public IList<long> StudentIds { get; set; }

    }
}
