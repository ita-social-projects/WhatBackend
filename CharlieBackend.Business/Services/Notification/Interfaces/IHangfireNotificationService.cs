using CharlieBackend.Business.Services.Notification.Enums;
using CharlieBackend.Core.Entities;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Notification.Interfaces
{
    public interface IHangfireNotificationService
    {
        Task CreateAddHomeworkNotification(long homeworkId, HomeworkInvokePeriod homeworkPeriod);

        Task CreateUpdateMarkNotification(long homeworkStudentId);

        Task CreateAddHomeworkStudentNotification(long homeworkStudentId);

        Task CreateUpdateHomeworkStudentNotification(long homeworkStudentId);

        Task CreateAddScheduledEventNotification(long scheduledEventId, ScheduledEventInvokePeriod scheduledEventPeriod);
    }
}
