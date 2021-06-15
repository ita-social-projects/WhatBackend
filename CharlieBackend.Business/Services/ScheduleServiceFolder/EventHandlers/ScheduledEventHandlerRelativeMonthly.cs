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
            var dayOfWeek = _pattern.DaysOfWeek[index];
            var nthDayOfWeekInTheMonth = GetNthDayOfWeekInTheMonth(_source.EventStart.Year, _source.EventStart.Month, dayOfWeek, _pattern.Index.Value);
            var startDate = nthDayOfWeekInTheMonth.Date
                .AddHours(_source.EventStart.Hour)
                .AddSeconds(_source.EventStart.Second);
            return startDate;
        }

        protected void UpdateTime(ref DateTime startDate, ref DateTime finishDate, int index)
        {
            startDate = startDate.Date.AddMonths(_pattern.Interval);
            var dayOfWeek = _pattern.DaysOfWeek[index];
            var nthDayOfWeekInTheMonth = GetNthDayOfWeekInTheMonth(startDate.Year, startDate.Month, dayOfWeek, _pattern.Index.Value);
            startDate = nthDayOfWeekInTheMonth.Date
                .AddHours(_source.EventStart.Hour)
                .AddSeconds(_source.EventStart.Second);
            finishDate = new DateTime(startDate.Year, startDate.Month, startDate.Day,
                    _source.EventFinish.Hour, _source.EventFinish.Minute, _source.EventFinish.Second);
        }

        protected override int GetIterationCount()
        {
            return _pattern.DaysOfWeek.Count;
        }

        private DateTime GetFirstDayOfWeekInTheMonth(int year, int month, DayOfWeek dayOfWeek)
        {
            var firstDayOfTheMonth = new DateTime(year, month, 1);
            var offset = 0;

            // if the first week of the month does not have this day of week
            // then move forward to the nex week and back on difference between days 
            if (firstDayOfTheMonth.DayOfWeek > dayOfWeek)
            {
                offset = DAYS_IN_WEEK - (firstDayOfTheMonth.DayOfWeek - dayOfWeek);
            }

            // if the first week of the month has this day of week
            // then move forward on difference between days
            if (firstDayOfTheMonth.DayOfWeek < dayOfWeek)
            {
                offset = dayOfWeek - firstDayOfTheMonth.DayOfWeek;
            }

            var firstDayOfWeekInTheMonth = firstDayOfTheMonth.AddDays(offset);
            return firstDayOfWeekInTheMonth;
        }

        private DateTime GetNthDayOfWeekInTheMonth(int year, int month, DayOfWeek dayOfWeek, WeekIndex n)
        {
            var firstDayOfWeekInTheMonth = GetFirstDayOfWeekInTheMonth(year, month, dayOfWeek);
            if (n == WeekIndex.Undefined || n == WeekIndex.First)
            {
                return firstDayOfWeekInTheMonth;
            }

            var nthDayOfWeekInTheMonth = firstDayOfWeekInTheMonth.AddDays(DAYS_IN_WEEK * ((int) n - 1));
            if (nthDayOfWeekInTheMonth.Month != firstDayOfWeekInTheMonth.Month)
            {
                nthDayOfWeekInTheMonth = nthDayOfWeekInTheMonth.AddDays(-DAYS_IN_WEEK);
            }

            return nthDayOfWeekInTheMonth;

        }
    }
}
