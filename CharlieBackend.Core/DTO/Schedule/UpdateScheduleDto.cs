using CharlieBackend.Core.Entities;
using System;

namespace CharlieBackend.Core.DTO.Schedule
{
    public class UpdateScheduleDto
    {
        public TimeSpan LessonStart { get; set; }

        public TimeSpan LessonEnd { get; set; }

        public PatternType RepeatRate { get; set; }

        public uint? DayNumber { get; set; }
    }
}

