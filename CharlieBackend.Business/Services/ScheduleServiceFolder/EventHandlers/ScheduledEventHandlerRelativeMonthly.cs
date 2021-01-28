using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Business.Services.ScheduleServiceFolder
{
    class ScheduledEventHandlerRelativeMonthly : ScheduledEventHandlerBase
    {
        public ScheduledEventHandlerRelativeMonthly(EventOccurence source, ContextForCreateScheduleDTO context, PatternForCreateScheduleDTO pattern) 
            : base(source, context, pattern)
        {
        }

        public override IEnumerable<ScheduledEvent> GetEvents()
        {
            int count = GetIterationCount();

            for (int i = 0; i < count; i++)
            {
                DateTime targetStartDate = GetStartDate(i);

                DateTime targetFinishDate = new DateTime(targetStartDate.Year, targetStartDate.Month, targetStartDate.Day,
                    _source.EventFinish.Value.Hour, _source.EventFinish.Value.Minute, _source.EventFinish.Value.Second);

                while (targetFinishDate <= _source.EventFinish)
                {
                    yield return CreateEvent(targetStartDate, targetFinishDate, _source, _context);

                    UpdateTime(ref targetStartDate, ref targetFinishDate, i);
                }
            }
        }

        public override DateTime GetStartDate(int index)
        {
            DateTime startDate = new DateTime(_source.EventStart.Year, _source.EventStart.Month, 7 * (int)_pattern.Index,
                _source.EventStart.Hour, _source.EventStart.Minute, _source.EventStart.Second); 

            return startDate.AddDays(startDate.DayOfWeek <= _pattern.DaysOfWeek[index]
                       ? _pattern.DaysOfWeek[index] - startDate.DayOfWeek
                       : 7 - (int)startDate.DayOfWeek + (int)_pattern.DaysOfWeek[index]);
        }

        public void UpdateTime(ref DateTime startTime, ref DateTime finishDate, int index)
        {
            startTime = startTime.AddMonths(_pattern.Interval);

            startTime = new DateTime(startTime.Year, startTime.Month, 7 * (int)_pattern.Index,
                startTime.Hour, startTime.Minute, startTime.Second);

            startTime = startTime.AddDays(startTime.DayOfWeek <= _pattern.DaysOfWeek[index]
                       ? _pattern.DaysOfWeek[index] - startTime.DayOfWeek
                       : 7 - (int)startTime.DayOfWeek + (int)_pattern.DaysOfWeek[index]);

            finishDate = new DateTime(startTime.Year, startTime.Month, startTime.Day,
                    _source.EventFinish.Value.Hour, _source.EventFinish.Value.Minute, _source.EventFinish.Value.Second);
        }

        public override int GetIterationCount()
        {
            return _pattern.DaysOfWeek.Count;
        }
    }
}
