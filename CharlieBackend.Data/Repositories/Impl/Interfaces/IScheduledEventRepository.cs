using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IScheduledEventRepository : IRepository<ScheduledEvent>
    {
        Task<IList<ScheduledEvent>> GetEventsFilteredAsync(ScheduledEventFilterRequestDTO request);

        Task<bool> IsLessonConnectedToSheduledEventAsync(long lessonId);

        Task<ScheduledEvent> ConnectEventToLessonByIdAsync(long? eventId, long? lessonId);
    }
}
