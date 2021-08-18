using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using CharlieBackend.Core.DTO.Schedule;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IScheduledEventRepository : IRepository<ScheduledEvent>
    {
        Task<IList<ScheduledEvent>> GetEventsFilteredAsync(ScheduledEventFilterRequestDTO request);

        Task<ScheduledEvent> ConnectEventToLessonById(long? eventId, long? lessonId);
    }
}
