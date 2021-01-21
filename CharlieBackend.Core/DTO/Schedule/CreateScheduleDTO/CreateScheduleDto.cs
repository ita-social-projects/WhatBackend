using System;
using System.Collections.Generic;
using System.Text;
using CharlieBackend.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.DTO.Schedule
{
    public class CreateScheduleDto
    {
        [Required]
        public PatternForCreateScheduleDTO Pattern { get; set; }

        [Required]
        public OccurenceRange Range { get; set; }

        [Required]
        public ContextForCreateScheduleDTO Context { get; set; }
    }
}

