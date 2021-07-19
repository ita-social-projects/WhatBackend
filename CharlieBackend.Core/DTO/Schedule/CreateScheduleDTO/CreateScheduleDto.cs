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
        public PatternForCreateScheduleDTO Pattern { get; set; }

        public OccurenceRange Range { get; set; }

        public ContextForCreateScheduleDTO Context { get; set; }
    }
}

