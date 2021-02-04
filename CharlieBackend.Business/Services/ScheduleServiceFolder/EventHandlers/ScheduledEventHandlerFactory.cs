using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CharlieBackend.Core.Models.ResultModel;

namespace CharlieBackend.Business.Services.ScheduleServiceFolder
{
    public class ScheduledEventHandlerFactory
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
