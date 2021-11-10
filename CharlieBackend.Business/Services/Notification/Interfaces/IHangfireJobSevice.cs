using CharlieBackend.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Notification.Interfaces
{
    public interface IHangfireJobService
    {
        Task<bool> CreateAddHomeworkJob(Homework homework);

        Task<bool> CreateUpdateHomeworkJob(Homework homework);

        Task<bool> CreateAddHomeworkStudentJob(HomeworkStudent homeworkStudent);

        Task<bool> CreateUpdateHomeworkStudentJob(HomeworkStudent homeworkStudent);

        Task<bool> CreateUpdateMarkJob(HomeworkStudent homeworkStudent);

        Task<bool> CreateAddScheduledEventsJob(IEnumerable<ScheduledEvent> scheduledEvents);

        Task<bool> CreateUpdateScheduledEventsJob(IEnumerable<ScheduledEvent> scheduledEvents);

        Task<bool> CreateUpdateEventOccurrenceJob(IEnumerable<ScheduledEvent> removedScheduledEvents,
            IEnumerable<ScheduledEvent> scheduledEvents);

        Task<bool> DeleteScheduledEventsJob(IEnumerable<ScheduledEvent> scheduledEvents);
    }
}
