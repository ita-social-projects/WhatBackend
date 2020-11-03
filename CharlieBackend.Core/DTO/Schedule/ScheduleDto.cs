using System;
using System.Collections.Generic;
using System.Text;
using CharlieBackend.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.DTO.Schedule
{
    public class ScheduleDto
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public long StudentGroupId { get; set; }

        [Required]
        [RegularExpression(@"((([0-1][0-9])|(2[0-3]))(:[0-5][0-9])(:[0-5][0-9])?)", 
            ErrorMessage = "Time must be between 00:00 to 23:59")]
        public TimeSpan LessonStart { get; set; }

        [Required]
        [RegularExpression(@"((([0-1][0-9])|(2[0-3]))(:[0-5][0-9])(:[0-5][0-9])?)", 
            ErrorMessage = "Time must be between 00:00 to 23:59")]
        public TimeSpan LessonEnd { get; set; }

        [Required]  
        [EnumDataType(typeof(RepeatRate))]
        public RepeatRate RepeatRate { get; set; }

        [Range(1, 31)]   
        public uint? DayNumber { get; set; }
    }
}

