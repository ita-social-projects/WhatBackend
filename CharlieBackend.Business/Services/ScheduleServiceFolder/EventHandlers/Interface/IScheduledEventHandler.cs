using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Schedule;
using System.Collections.Generic;

namespace CharlieBackend.Business.Services.ScheduleServiceFolder
{
    public interface IScheduledEventHandler
    {
        IEnumerable<ScheduledEvent> GetEvents(EventOccurrence source, ContextForCreateScheduleDTO context);
    }
}
