using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CharlieBackend.Core.Models
{
    public class MentorModel
    {
        [Required]
        public string login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int[] courses_id { get; set; }
    }
}
