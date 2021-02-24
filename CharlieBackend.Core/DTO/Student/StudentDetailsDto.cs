using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Student
{
    public class StudentDetailsDto : StudentDto
    {
        public string AvatarUrl { get; set; }

    }
}
