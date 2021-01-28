using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Business.Services.ScheduleServiceFolder
{
    public abstract class ScheduledEventHandlerBase : IScheduledEventHandler
    {
        protected EventOccurence _source;
        protected ContextForCreateScheduleDTO _context;
        protected PatternForCreateScheduleDTO _pattern;
        protected int _index = 1;


        public ScheduledEventHandlerBase(EventOccurence source, ContextForCreateScheduleDTO context, PatternForCreateScheduleDTO pattern)
        {
            _source = source;
            _context = context;
            _pattern = pattern;
        }

        public virtual IEnumerable<ScheduledEvent> GetEvents()
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

                    UpdateTime(ref targetStartDate, ref targetFinishDate);
                }
            }
        }

        public ScheduledEvent CreateEvent(DateTime targetStartDate, DateTime targetFinishDate,
            EventOccurence source, ContextForCreateScheduleDTO context)
        {
            return new ScheduledEvent
            {
                EventOccurence = source,
                EventOccurenceId = source.Id,
                StudentGroupId = source.StudentGroupId,
                EventStart = targetStartDate,
                EventFinish = targetFinishDate,
                MentorId = context.MentorID,
                ThemeId = context.ThemeID
            };
        }

        public virtual int GetIterationCount()
        {
            return 1;
        }

        public virtual DateTime GetStartDate(int index)
        {
            return _source.EventStart;
        }

        public virtual void UpdateTime(ref DateTime starttime, ref DateTime finishDate)
        {
            starttime = starttime.AddDays(_pattern.Interval * _index);
            finishDate = finishDate.AddDays(_pattern.Interval * _index);
        }

    }
}
