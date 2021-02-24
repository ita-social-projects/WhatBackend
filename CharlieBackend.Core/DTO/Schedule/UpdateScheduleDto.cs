using System;
using System.Collections.Generic;
using System.Text;
using CharlieBackend.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.DTO.Schedule
{
    public class UpdateScheduleDto
    {
        [Required]
        [DataType(DataType.Time)]
        public TimeSpan LessonStart { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan LessonEnd { get; set; }

        [Required]  
        [EnumDataType(typeof(PatternType))]
        public PatternType RepeatRate { get; set; }

        [Range(1, 31)]   
        public uint? DayNumber { get; set; }
    }
}

