using System;
using System.Collections.Generic;
using System.Text;
using CharlieBackend.Business.Services.ScheduleServiceFolder;
using CharlieBackend.Core.DTO.Schedule;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IScheduledEventHandlerFactory
    {
        public IScheduledEventHandler Get(PatternForCreateScheduleDTO pattern);
    }
}
