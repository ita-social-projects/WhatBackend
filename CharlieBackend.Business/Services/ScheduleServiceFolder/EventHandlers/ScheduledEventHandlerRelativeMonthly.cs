using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Business.Services.ScheduleServiceFolder
{
    public class ScheduledEventHandlerRelativeMonthly : ScheduledEventHandlerBase
    {
        public ScheduledEventHandlerRelativeMonthly(EventOccurrence source, ContextForCreateScheduleDTO context, PatternForCreateScheduleDTO pattern) 
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

                while (targetFinishDate <= _source.EventFinish && targetStartDate >= _source.EventStart)
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

            int offset = _pattern.DaysOfWeek[index] == DayOfWeek.Sunday ? 7 : (int)_pattern.DaysOfWeek[index];
            int startDay = (int)startDate.DayOfWeek;

            return startDate.AddDays(startDay <= offset
                       ? -1 * (startDay + 7 - offset)
                       : -1 * (startDay - offset));
        }

        public void UpdateTime(ref DateTime startDate, ref DateTime finishDate, int index)
        {
            startDate = startDate.AddMonths(_pattern.Interval);

            startDate = new DateTime(startDate.Year, startDate.Month, 7 * (int)_pattern.Index,
                startDate.Hour, startDate.Minute, startDate.Second);

            int offset = _pattern.DaysOfWeek[index] == DayOfWeek.Sunday ? 7 : (int)_pattern.DaysOfWeek[index];
            int startDay = (int)startDate.DayOfWeek;

            startDate = startDate.AddDays(startDay <= offset
                       ? -1 * (startDay + 7 - offset)
                       : -1 * (startDay - offset));

            finishDate = new DateTime(startDate.Year, startDate.Month, startDate.Day,
                    _source.EventFinish.Value.Hour, _source.EventFinish.Value.Minute, _source.EventFinish.Value.Second);
        }

        public override int GetIterationCount()
        {
            return _pattern.DaysOfWeek.Count;
        }
    }
}
