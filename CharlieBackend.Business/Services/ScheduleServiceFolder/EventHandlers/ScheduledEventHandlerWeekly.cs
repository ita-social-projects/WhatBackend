using CharlieBackend.Core.DTO.Schedule;
using System;

namespace CharlieBackend.Business.Services.ScheduleServiceFolder
{
    public class ScheduledEventHandlerWeekly : ScheduledEventHandlerBase
    {
        public ScheduledEventHandlerWeekly(PatternForCreateScheduleDTO pattern)
            : base(pattern)
        {
            _index = 7;
        }

        protected override DateTime GetStartDate(int index)
        {
            DateTime result = _source.EventStart;
            
            return result.AddDays(result.DayOfWeek <= _pattern.DaysOfWeek[index]
                    ? _pattern.DaysOfWeek[index] - result.DayOfWeek
                    : 7 - (int)result.DayOfWeek + (int)_pattern.DaysOfWeek[index]);
        }

        protected override int GetIterationCount()
        {
            return _pattern.DaysOfWeek.Count;
        }
    }
}
