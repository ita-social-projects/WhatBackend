
﻿using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.Course
{
    public class CreateCourseModel
    {
        [Required]
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
