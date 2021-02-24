using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CharlieBackend.Core.DTO.Mentor
{
    public class MentorDetailsDto : MentorDto
    {
        public string AvatarUrl { get; set; }
    }
}
