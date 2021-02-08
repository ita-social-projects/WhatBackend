using System;
using System.Collections.Generic;
using System.Text;
using CharlieBackend.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.DTO.Schedule
{
    public class ScheduledEventDTO
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public long EventOccuranceId { get; set; }

        [Required]
        public long StudentGroupId { get; set; }

        public long? ThemeId { get; set; }

        public long? MentorId { get; set; }

        public long? LessonId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EventStart { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EventFinish { get; set; }
    }
}
