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
        public DateTime LessonStart { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime LessonEnd { get; set; }

        [Required]  
        [EnumDataType(typeof(RepeatRate))]
        public RepeatRate RepeatRate { get; set; }

        [Range(1, 31)]   
        public uint? DayNumber { get; set; }
    }
}

