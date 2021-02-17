using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Business.Services.ScheduleServiceFolder
{
    public class ScheduledEventHandlerRelativeMonthly : ScheduledEventHandlerBase
    {
        public ScheduledEventHandlerRelativeMonthly(PatternForCreateScheduleDTO pattern) 
            : base(pattern)
        {
        }

        public override IEnumerable<ScheduledEvent> GetEvents(EventOccurrence source, ContextForCreateScheduleDTO context)
        {
            _context = context;
            _source = source;

            int count = GetIterationCount();

            for (int i = 0; i < count; i++)
            {
                DateTime targetStartDate = GetStartDate(i);

                DateTime targetFinishDate = new DateTime(targetStartDate.Year, targetStartDate.Month, targetStartDate.Day,
                    _source.EventFinish.Hour, _source.EventFinish.Minute, _source.EventFinish.Second);

                while (targetFinishDate <= _source.EventFinish && targetStartDate >= _source.EventStart)
                {
                    yield return new ScheduledEvent
                    {
                        EventOccurrence = _source,
                        EventOccurrenceId = _source.Id,
                        StudentGroupId = _source.StudentGroupId,
                        EventStart = targetStartDate,
                        EventFinish = targetFinishDate,
                        MentorId = _context.MentorID,
                        ThemeId = _context.ThemeID
                    }; ;

                    UpdateTime(ref targetStartDate, ref targetFinishDate, i);
                }
            }
        }

        protected override DateTime GetStartDate(int index)
        {
            DateTime startDate = new DateTime(_source.EventStart.Year, _source.EventStart.Month, 7 * (int)_pattern.Index,
                _source.EventStart.Hour, _source.EventStart.Minute, _source.EventStart.Second);

            int offset = _pattern.DaysOfWeek[index] == DayOfWeek.Sunday ? 7 : (int)_pattern.DaysOfWeek[index];
            int startDay = (int)startDate.DayOfWeek;

            return startDate.AddDays(startDay <= offset
                       ? -1 * (startDay + DAYS_IN_WEEK - offset)
                       : -1 * (startDay - offset));
        }

        protected void UpdateTime(ref DateTime startDate, ref DateTime finishDate, int index)
        {
            startDate = startDate.AddMonths(_pattern.Interval);

            startDate = new DateTime(startDate.Year, startDate.Month, 7 * (int)_pattern.Index,
                startDate.Hour, startDate.Minute, startDate.Second);

            int offset = _pattern.DaysOfWeek[index] == DayOfWeek.Sunday ? 7 : (int)_pattern.DaysOfWeek[index];
            int startDay = (int)startDate.DayOfWeek;

            startDate = startDate.AddDays(startDay <= offset
                       ? -1 * (startDay + DAYS_IN_WEEK - offset)
                       : -1 * (startDay - offset));

            finishDate = new DateTime(startDate.Year, startDate.Month, startDate.Day,
                    _source.EventFinish.Hour, _source.EventFinish.Minute, _source.EventFinish.Second);
        }

        protected override int GetIterationCount()
        {
            return _pattern.DaysOfWeek.Count;
        }
    }
}
