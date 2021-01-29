using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Business.Services.ScheduleServiceFolder
{
    public abstract class ScheduledEventHandlerBase : IScheduledEventHandler
    {
        protected EventOccurrence _source;
        protected ContextForCreateScheduleDTO _context;
        protected PatternForCreateScheduleDTO _pattern;
        protected int _index = 1;

        public ScheduledEventHandlerBase(PatternForCreateScheduleDTO pattern)
        {
            _pattern = pattern;
        }

        public virtual IEnumerable<ScheduledEvent> GetEvents(EventOccurrence source, ContextForCreateScheduleDTO context)
        {
            _context = context;
            _source = source;

            int count = GetIterationCount();

            for (int i = 0; i < count; i++)
            {
                DateTime targetStartDate = GetStartDate(i);

                DateTime targetFinishDate = new DateTime(targetStartDate.Year, targetStartDate.Month, targetStartDate.Day,
                    _source.EventFinish.Hour, _source.EventFinish.Minute, _source.EventFinish.Second);

                while (targetFinishDate <= _source.EventFinish)
                {
                    yield return new ScheduledEvent
                    {
                        EventOccurenceId = _source.Id,
                        StudentGroupId = _source.StudentGroupId,
                        EventStart = targetStartDate,
                        EventFinish = targetFinishDate,
                        MentorId = _context.MentorID,
                        ThemeId = _context.ThemeID
                    };

                    UpdateTime(ref targetStartDate, ref targetFinishDate);
                }
            }
        }

        protected virtual int GetIterationCount()
        {
            return 1;
        }

        protected virtual DateTime GetStartDate(int index)
        {
            return _source.EventStart;
        }

        protected virtual void UpdateTime(ref DateTime startDate, ref DateTime finishDate)
        {
            startDate = startDate.AddDays(_pattern.Interval * _index);
            finishDate = finishDate.AddDays(_pattern.Interval * _index);
        }

    }
}
