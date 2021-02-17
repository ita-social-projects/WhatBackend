using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Schedule
{
    public class OccurenceRange
    {
        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? FinishDate { get; set; }
    }
}
