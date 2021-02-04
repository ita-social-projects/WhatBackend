using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Business.Services.ScheduleServiceFolder
{
    public class ScheduledEventHandlerAbsoluteMonthly : ScheduledEventHandlerBase
    {
        public ScheduledEventHandlerAbsoluteMonthly(PatternForCreateScheduleDTO pattern)
            : base(pattern)
        {
            
        }

        protected override DateTime GetStartDate(int index)
        {
            DateTime startDate = _source.EventStart;

            return startDate.Day <= _pattern.Dates[index] ?
                    new DateTime(startDate.Year, startDate.Month, _pattern.Dates[index],
                    startDate.Hour, startDate.Minute, startDate.Second) :
                    new DateTime(startDate.Year, startDate.Month, _pattern.Dates[index],
                    startDate.Hour, startDate.Minute, startDate.Second).AddMonths(1);
        }

        protected override void UpdateTime(ref DateTime startDate, ref DateTime finishDate)
        {
            startDate = startDate.AddDays(_pattern.Interval);
            finishDate = finishDate.AddDays(_pattern.Interval);
        }
    }
}
