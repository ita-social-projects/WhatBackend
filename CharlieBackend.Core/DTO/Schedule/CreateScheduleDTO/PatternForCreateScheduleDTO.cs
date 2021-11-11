using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;

namespace CharlieBackend.Core.DTO.Schedule
{
    public class PatternForCreateScheduleDTO
    {
        public PatternType Type { get; set; }

        public int Interval { get; set; }

        public IList<DayOfWeek> DaysOfWeek { get; set; }

        public MonthIndex? Index { get; set; }

        public IList<int> Dates { get; set; }
    }
}
