using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Schedule
{
    public class ContextForCreateScheduleDTO
    {
        [Required]
        public long GroupID { get; set; }

        public long? ThemeID { get; set; }

        public long? MentorID { get; set; }
    }
}
