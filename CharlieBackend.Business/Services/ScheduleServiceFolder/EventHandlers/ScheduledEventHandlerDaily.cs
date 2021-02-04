using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Business.Services.ScheduleServiceFolder
{
    public class ScheduledEventHandlerDaily : ScheduledEventHandlerBase
    {
        public ScheduledEventHandlerDaily(PatternForCreateScheduleDTO pattern)
            : base(pattern)
        {
        }
    }
}
