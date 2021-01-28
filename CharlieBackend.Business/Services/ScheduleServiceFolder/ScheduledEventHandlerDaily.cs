using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Business.Services.ScheduleServiceFolder
{
    class ScheduledEventHandlerDaily : ScheduledEventHandlerBase
    {
        public ScheduledEventHandlerDaily(EventOccurence source, ContextForCreateScheduleDTO context, PatternForCreateScheduleDTO pattern) : base(source, context, pattern)
        {
        }
    }
}
