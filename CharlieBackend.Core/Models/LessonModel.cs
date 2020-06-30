using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CharlieBackend.Core.Models
{
    public class LessonModel
    {
        public long Id { get; set; }
        [Required]
        public string name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public string lesson_date { get; set; }

        [Required]
        public long group_id { get; set; }
    }
}
