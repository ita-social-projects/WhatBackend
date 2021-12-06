using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Business.Services.ScheduleServiceFolder;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using System;

namespace CharlieBackend.Business.Services
{
    public class ScheduledEventHandlerFactory : IScheduledEventHandlerFactory
    {
        public IScheduledEventHandler Get(PatternForCreateScheduleDTO pattern)
        {
            return pattern.Type switch
            {
                PatternType.Daily => new ScheduledEventHandlerDaily(pattern),
                PatternType.Weekly => new ScheduledEventHandlerWeekly(pattern),
                PatternType.AbsoluteMonthly => new ScheduledEventHandlerAbsoluteMonthly(pattern),
                PatternType.RelativeMonthly => new ScheduledEventHandlerRelativeMonthly(pattern),
                _ => throw new NotImplementedException($"{pattern.Type} not implemented")
            };
        }
    }
}
