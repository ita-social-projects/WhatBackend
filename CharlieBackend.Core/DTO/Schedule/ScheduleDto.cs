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
        [DataType(DataType.Time)]
        public TimeSpan LessonStart { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan LessonEnd { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [EnumDataType(typeof(RepeatRate))]
        public RepeatRate RepeatRate { get; set; }

        [Range(1, 31)]   
        public uint? DayNumber { get; set; }
    }
}

